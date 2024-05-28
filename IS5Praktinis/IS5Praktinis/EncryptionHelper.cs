using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly int KeySize = 32; 
    public static readonly string EncryptionKeyFilePath = "encryptionkey.txt";
    public static string EncryptionKey { get; private set; } = LoadOrGenerateKey();

    private static string LoadOrGenerateKey()
    {
        if (File.Exists(EncryptionKeyFilePath))
        {
            return File.ReadAllText(EncryptionKeyFilePath);
        }
        else
        {
            var key = GenerateRandomKey();
            File.WriteAllText(EncryptionKeyFilePath, key);
            return key;
        }
    }

    private static string GenerateRandomKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var key = new byte[KeySize];
            rng.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }

    public static void EncryptFile(string inputFile, string outputFile, string key)
    {
        File.AppendAllText("app_log.txt", $"Encrypting file {inputFile} at {DateTime.Now}\n");

        byte[] keyBytes = Convert.FromBase64String(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV(); 

            using (var fileStream = new FileStream(outputFile, FileMode.Create))
            {
                fileStream.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int)); 
                fileStream.Write(aes.IV, 0, aes.IV.Length); 

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var inputStream = new FileStream(inputFile, FileMode.Open))
                        {
                            inputStream.CopyTo(cryptoStream);
                        }
                    }
                }
            }
        }

        File.AppendAllText("app_log.txt", $"File {inputFile} encrypted at {DateTime.Now}\n");
    }
    public static void DecryptFile(string inputFile, string outputFile, string key)
    {
        File.AppendAllText("app_log.txt", $"Decrypting file {inputFile} at {DateTime.Now}\n");

        byte[] keyBytes = Convert.FromBase64String(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;

            using (var fileStream = new FileStream(inputFile, FileMode.Open))
            {
                byte[] ivLengthBytes = new byte[sizeof(int)];
                fileStream.Read(ivLengthBytes, 0, sizeof(int)); 
                int ivLength = BitConverter.ToInt32(ivLengthBytes, 0);

                byte[] iv = new byte[ivLength];
                fileStream.Read(iv, 0, iv.Length); 
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var outputStream = new FileStream(outputFile, FileMode.Create))
                        {
                            cryptoStream.CopyTo(outputStream);
                        }
                    }
                }
            }
        }

        File.AppendAllText("app_log.txt", $"File {inputFile} decrypted at {DateTime.Now}\n");
    }

    public static string Encrypt(string clearText, string key)
    {
        byte[] iv;
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV();
            iv = aes.IV;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(clearText);
                        }
                        array = ms.ToArray();
                    }
                }
            }
        }

        var result = new byte[iv.Length + array.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(array, 0, result, iv.Length, array.Length);
        return Convert.ToBase64String(result);
    }

    public static string Decrypt(string cipherText, string key)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);

            var iv = new byte[aes.BlockSize / 8];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            {
                using (var ms = new MemoryStream(cipher))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public static string GenerateRandomPassword(int length = 12)
    {
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }
        }
        return res.ToString();
    }
}
