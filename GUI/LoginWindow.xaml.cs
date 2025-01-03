using System.Threading.Tasks;
using System.Windows;
using _21880263.BUS;
using _21880263.GUI;

namespace _21880263
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Login_BUS? Login_BUS { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            if (Login_BUS == null) Login_BUS = new Login_BUS();
            Login_BUS.ReloadSetting();
            if (!string.IsNullOrEmpty(Login_BUS.UserID)) usernameTextBox.Text = Login_BUS.UserID.ToString();
            passwordTextBox.Password = Login_BUS.Password;
            chbRememberPassword.IsChecked = Login_BUS.RememberPassword;
            serverTextBox.Text = Login_BUS.Server;
            databaseTextBox.Text = Login_BUS.Database;
            DataContext = this;
            if (chbRememberPassword.IsChecked == true && !string.IsNullOrEmpty(passwordTextBox.Password))
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    await _login();
                });
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            await _login();
        }

        private async Task _login()
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text) || string.IsNullOrEmpty(passwordTextBox.Password))
            {
                MyMessageBox.Show("Username & Password is required!", "Warning!");
                return;
            }
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(loginButton, true);
            if (Login_BUS != null)
            {
                if (!string.IsNullOrEmpty(Login_BUS.UserID)) Login_BUS.UserID = usernameTextBox.Text;
                Login_BUS.Password = passwordTextBox.Password;
                Login_BUS.UserID = usernameTextBox.Text;
                Login_BUS.Server = serverTextBox.Text;
                Login_BUS.Database = databaseTextBox.Text;
                if (chbRememberPassword.IsChecked != null) Login_BUS.RememberPassword = chbRememberPassword.IsChecked.Value;
                bool success = await Task.Run(() =>
                {
                    bool success = true;
                    success = Login_BUS.Login();
                    return success;
                });
                if (success)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.UserID = Login_BUS.UserID;
                    this.Dispatcher.Invoke(() => mainWindow.Show());
                    this.Close();
                }
                else
                {
                    MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(loginButton, false);
                    if (!MyMessageBox.Show("Connect Failed. Your acount or server is wrong. Please check again!", "Error!")) { Application.Current.Shutdown(); }
                }
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (Login_BUS != null) Login_BUS.SaveServerConfig(serverTextBox.Text, databaseTextBox.Text);
            serverPopup.IsPopupOpen = false;
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            serverPopup.IsPopupOpen = false;
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
