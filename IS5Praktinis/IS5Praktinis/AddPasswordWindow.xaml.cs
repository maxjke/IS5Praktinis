using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace IS5Praktinis
{
    public partial class AddPasswordWindow : Window
    {
        private string passwordFilePath;

        public AddPasswordWindow(string username)
        {
            InitializeComponent();
            passwordFilePath = $"{username}_passwords.txt";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text;
            var password = PasswordBox.Password;
            var url = UrlTextBox.Text;
            var comment = CommentTextBox.Text;

            var encryptedPassword = EncryptionHelper.Encrypt(password, EncryptionHelper.EncryptionKey);

            var txtLine = $"{title},{encryptedPassword},{url},{comment}";
            File.AppendAllText(passwordFilePath, txtLine + Environment.NewLine);

            MessageBox.Show("Password saved successfully!");
            Close();
        }

        private void GeneratePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var randomPassword = EncryptionHelper.GenerateRandomPassword();
            PasswordBox.Password = randomPassword;
        }

        private void CopyPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            if (!string.IsNullOrEmpty(password))
            {
                Clipboard.SetText(password);
                MessageBox.Show("Password copied to clipboard!");
            }
            else
            {
                MessageBox.Show("Password field is empty.");
            }
        }
    }
}
