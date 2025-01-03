using _21880263.DAO;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.BUS
{
    public class Login_BUS
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserID { get; set; }
        public string? Password { get; set; }

        public bool RememberPassword { get; set; }

        Configuration? config { get; set; }

        public void ReloadSetting()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            UserID = config.AppSettings.Settings["username"].Value;
            Server = config.AppSettings.Settings["Server"].Value ?? "";
            Database = config.AppSettings.Settings["Database"].Value ?? "";
            RememberPassword = config.AppSettings.Settings["RememberPassword"].Value == "1";
            if (RememberPassword)
            {
                var Passwordin64 = config.AppSettings.Settings["Password"].Value ?? "";
                var Entropyint64 = config.AppSettings.Settings["Entropy"].Value ?? "";
                if (Passwordin64.Length != 0)
                {
                    var passwordInBytes = Convert.FromBase64String(Passwordin64);
                    var entropyInBytes = Convert.FromBase64String(Entropyint64);
                    var unencryptedPassword = ProtectedData.Unprotect(passwordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                    Password = Encoding.UTF8.GetString(unencryptedPassword);
                }
            }
        }

        public bool Login()
        {
            //var builder = new SqlConnectionStringBuilder();
            //builder.DataSource = Server;
            //builder.InitialCatalog = Database;
            //builder.UserID = UserID;
            //builder.Password = Password;
            //builder.TrustServerCertificate = true;
            string connectionString = $"Data Source = {Server}; Initial Catalog = {Database}; User ID = {UserID}; Password = {Password};TrustServerCertificate=true;";
            EShopDbContext.SetConnectionString(connectionString);
            bool isCompletedSuccessfully = false;
            Task task = Task.Run(() => {
                isCompletedSuccessfully = EShopDbContext.Instance.Database.CanConnect();
            });
            if (task.Wait(TimeSpan.FromMilliseconds(5000)) && isCompletedSuccessfully)
            {
                if (config != null)
                {
                    config.AppSettings.Settings["RememberPassword"].Value = RememberPassword ? "1" : "";
                    if (RememberPassword)
                    {
                        if (Password != null)
                        {
                            var passwordInBytes = Encoding.UTF8.GetBytes(Password);
                            var entropy = new byte[20];
                            RandomNumberGenerator.Fill(entropy);
                            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
                            config.AppSettings.Settings["Password"].Value = Convert.ToBase64String(cypherText);
                            config.AppSettings.Settings["Entropy"].Value = Convert.ToBase64String(entropy);
                            config.AppSettings.Settings["Username"].Value = UserID;
                        }
                    }
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            return isCompletedSuccessfully;
            //return false;
        }

        public void SaveServerConfig(string Server, string Database)
        {
            if (config != null)
            {
                config.AppSettings.Settings["Server"].Value = Server;
                config.AppSettings.Settings["Database"].Value = Database;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            this.Server = Server;
            this.Database = Database;
        }
    }
}
