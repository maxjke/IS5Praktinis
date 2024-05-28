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
    /// Interaction logic for SearchPasswordWindow.xaml
    /// </summary>
    public partial class SearchPasswordWindow : Window
    {
        private string passwordFilePath;
        private string encryptedPassword;

        public SearchPasswordWindow(string username)
        {
            InitializeComponent();
            passwordFilePath = $"{username}_passwords.txt";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var title = SearchTitleTextBox.Text;
            var lines = File.ReadAllLines(passwordFilePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts[0] == title)
                {
                    ResultTextBlock.Text = $"Title: {parts[0]}\nPassword: ********\nURL: {parts[2]}\nComment: {parts[3]}";
                    encryptedPassword = parts[1];
                    ShowPasswordButton.Visibility = Visibility.Visible;
                    return;
                }
            }

            ResultTextBlock.Text = "Password not found.";
            ShowPasswordButton.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var decryptedPassword = EncryptionHelper.Decrypt(encryptedPassword, EncryptionHelper.EncryptionKey);
            ResultTextBlock.Text = ResultTextBlock.Text.Replace("********", decryptedPassword);
        }
    }


}
