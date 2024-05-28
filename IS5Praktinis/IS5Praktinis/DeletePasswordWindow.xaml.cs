using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for DeletePasswordWindow.xaml
    /// </summary>
    public partial class DeletePasswordWindow : Window
    {
        private string passwordFilePath;

        public DeletePasswordWindow(string username)
        {
            InitializeComponent();
            passwordFilePath = $"{username}_passwords.txt";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text;

            var lines = File.ReadAllLines(passwordFilePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts[0] == title)
                {
                    lines.RemoveAt(i);
                    File.WriteAllLines(passwordFilePath, lines);
                    MessageBox.Show("Password deleted successfully!");
                    Close();
                    return;
                }
            }

            MessageBox.Show("Password not found.");
        }
    }


}
