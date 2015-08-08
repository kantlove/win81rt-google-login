using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleLogin.Common;
using Google.Apis.Oauth2.v2;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoogleLogin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IWebAuthenticationContinuable
    {
        public static MainPage Current { get; private set; }
        private UserCredential _credential;
        private Oauth2Service _service;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            Current = this;
        }

        public async void ContinueWebAuthentication(
    WebAuthenticationBrokerContinuationEventArgs args)
        {
            await PasswordVaultDataStore.Default.StoreAsync<SerializableWebAuthResult>(
                SerializableWebAuthResult.Name,
                new SerializableWebAuthResult(args.WebAuthenticationResult));

            await GetInfo();

            await PasswordVaultDataStore.Default.DeleteAsync<SerializableWebAuthResult>(
                SerializableWebAuthResult.Name);
        }

        private async Task AuthenticateAsync()
        {
            if (_service != null)
                return;

            _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new Uri("ms-appx:///Assets/client_secrets.json"),
                new[] { Oauth2Service.Scope.UserinfoEmail },
                "user",
                CancellationToken.None);

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "GoogleLogin",
            };

            _service = new Oauth2Service(initializer);

        }

        private void GetFiles_Click(object sender, RoutedEventArgs e)
        {
            DoSomething();
            
        }

        async void DoSomething()
        {
            await AuthenticateAsync();
            
        }

        async Task GetInfo()
        {
            await AuthenticateAsync();

            var userinfo = await _service.Userinfo.Get().ExecuteAsync();
            tbMail.Text = userinfo.Email;
        }

    }
}
