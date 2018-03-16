using LicenseKey;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ActivateMe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtVersion.Text = "v" + typeof(MainWindow).Assembly.GetName().Version.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GenerateKey gkey;
            gkey = new GenerateKey();
            gkey.LicenseTemplate = "vvvvppppxxxxxxxxxxxx" +
               "-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx" +
               "-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
            gkey.MaxTokens = 2;
            gkey.AddToken(0, "v", LicenseKey.GenerateKey.TokenTypes.NUMBER, "0");
            gkey.AddToken(1, "p", LicenseKey.GenerateKey.TokenTypes.NUMBER, "0");
            gkey.UseBase10 = false;
            gkey.UseBytes = false;
            gkey.CreateKey();
            string finalkey = gkey.GetLicenseKey();
            if (RegistryHelper.GetKeyValue("adata") != null)
            {
                MessageBox.Show("Activation data already exists!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            RegistryHelper.SetKeyValue("adata", finalkey);
            RegistryHelper.SetKeyValue("ah", GetSha1Hash(finalkey));
            MessageBox.Show("Successfully saved activation data.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static string GetSha1Hash(string data)
        {
            SHA1 sha = new SHA1Managed();
            byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(data)); ;
            return Convert.ToBase64String(result);
        }
    }
}
