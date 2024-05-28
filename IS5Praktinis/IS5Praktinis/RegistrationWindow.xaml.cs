using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IS5Praktinis
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(password, salt);

            var userRecord = $"{username},{hashedPassword},{salt}";

            File.AppendAllText("users.txt", userRecord + Environment.NewLine);

            
            var passwordFilePath = $"{username}_passwords.txt";
            File.WriteAllText(passwordFilePath, string.Empty);
            EncryptionHelper.EncryptFile(passwordFilePath, $"{username}_passwords.enc", EncryptionHelper.EncryptionKey);

            MessageBox.Show("User registered successfully!");
            Close();
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                return Convert.ToBase64String(deriveBytes.GetBytes(256));
            }
        }
    }




}
