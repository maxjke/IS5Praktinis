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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS5Praktinis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentUser;
        private string passwordFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string username) 
        {
            currentUser = username;
            passwordFilePath = $"{currentUser}_passwords.txt";
        }

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var addPasswordWindow = new AddPasswordWindow(App.CurrentUser);
            addPasswordWindow.ShowDialog();
        }

        private void SearchPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var searchPasswordWindow = new SearchPasswordWindow(App.CurrentUser);
            searchPasswordWindow.ShowDialog();
        }

        private void UpdatePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var updatePasswordWindow = new UpdatePasswordWindow(App.CurrentUser);
            updatePasswordWindow.ShowDialog();
        }

        private void DeletePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var deletePasswordWindow = new DeletePasswordWindow(App.CurrentUser);
            deletePasswordWindow.ShowDialog();
        }
    }

}
