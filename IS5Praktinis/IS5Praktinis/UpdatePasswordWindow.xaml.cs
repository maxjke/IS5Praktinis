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
    /// Interaction logic for UpdatePasswordWindow.xaml
    /// </summary>
    public partial class UpdatePasswordWindow : Window
    {
        private string passwordFilePath;

        public UpdatePasswordWindow(string username)
        {
            InitializeComponent();
            passwordFilePath = $"{username}_passwords.txt";
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text;
            var newPassword = NewPasswordBox.Password;
            var encryptedNewPassword = EncryptionHelper.Encrypt(newPassword, EncryptionHelper.EncryptionKey);

            var lines = File.ReadAllLines(passwordFilePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts[0] == title)
                {
                    lines[i] = $"{parts[0]},{encryptedNewPassword},{parts[2]},{parts[3]}";
                    File.WriteAllLines(passwordFilePath, lines);
                    MessageBox.Show("Password updated successfully!");
                    Close();
                    return;
                }
            }

            MessageBox.Show("Password not found.");
        }
    }


}
