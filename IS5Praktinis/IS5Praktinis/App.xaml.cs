using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace IS5Praktinis
{
    public partial class App : Application
    {
        private const string UserFilePath = "users.txt";
        public static string CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!File.Exists(UserFilePath))
            {
                File.WriteAllText(UserFilePath, string.Empty);
            }

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (!string.IsNullOrEmpty(CurrentUser))
            {
                var mainWindow = new MainWindow(CurrentUser);
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (!string.IsNullOrEmpty(CurrentUser))
            {
                var passwordFilePath = $"{CurrentUser}_passwords.txt";
                var encryptedPasswordFilePath = $"{CurrentUser}_passwords.enc";

                if (File.Exists(passwordFilePath))
                {
                    try
                    {
                        EncryptionHelper.EncryptFile(passwordFilePath, encryptedPasswordFilePath, EncryptionHelper.EncryptionKey);
                        File.Delete(passwordFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred during encryption: {ex.Message}");
                    }
                }
            }

            
            File.AppendAllText("app_log.txt", $"Application exited at {DateTime.Now}\n");
        }
    }
}
