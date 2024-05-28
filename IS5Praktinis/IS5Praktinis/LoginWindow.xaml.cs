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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string CurrentUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            if (File.Exists("users.txt"))
            {
                var users = File.ReadAllLines("users.txt");

                foreach (var user in users)
                {
                    var parts = user.Split(',');
                    if (parts[0] == username && VerifyPassword(password, parts[1], parts[2]))
                    {
                        CurrentUser = username;
                        var passwordFilePath = $"{CurrentUser}_passwords.enc";
                        if (File.Exists(passwordFilePath))
                        {
                            EncryptionHelper.DecryptFile(passwordFilePath, $"{username}_passwords.txt", EncryptionHelper.EncryptionKey);
                        }
                        MessageBox.Show("Login successful!");

                        
                        App.CurrentUser = CurrentUser;

                       
                        var mainWindow = new MainWindow(CurrentUser);
                        mainWindow.Show();
                        this.Close();
                        return;
                    }
                }
                

                MessageBox.Show("Invalid username or password.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private bool VerifyPassword(string password, string storedHash, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                var hash = Convert.ToBase64String(deriveBytes.GetBytes(256));
                return hash == storedHash;
            }
        }
    }





}
