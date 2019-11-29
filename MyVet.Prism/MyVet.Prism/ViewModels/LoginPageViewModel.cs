using System;
using MyVet.Common.Models;
using MyVet.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        #region PrivateAttrbs
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand; 
        #endregion

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            isEnabled = true;
        }

        #region PublicProps
        //public DelegateCommand Logincommand
        //{
        //    get
        //    {
        //        if (_loginCommand == null)
        //        {
        //            _loginCommand = new DelegateCommand(Login);
        //        }
        //        return _loginCommand;
        //    }
        //}

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));
        public string Email { get; set; }

        //Los campos o parametros que cambian en la view, se les crea una 
        //propiedad publica y un atributo privado
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public bool isRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool isEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        } 
        #endregion
       
        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an email.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a password.", "Accept");
                return;
            }

            isRunning = true;
            isEnabled = false;

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
                                                            //URL  Prefijo  Accion
            var response = await _apiService.GetTokenAsync(url, "/Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                isRunning = false;
                isEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Email or password invalid.", "Accept");
                Password = string.Empty;
                return;
            }

            var token = (TokenResponse)response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url, "api", "/Owners/GetOwnerByEmail", "bearer", token.Token, Email);

            if (!response2.IsSuccess)
            {
                isRunning = false;
                isEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "There's a problem.", "Accept");
                Password = string.Empty;
                return;
            }

            var owner = (OwnerResponse)response2.Result;
            //enviar parametros de esta pagina a otra
            var parameters = new NavigationParameters 
            {
                { "owner", owner}
            };
            isRunning = false; 
            isEnabled = true;
            Password = string.Empty;
            await _navigationService.NavigateAsync("PetsPage", parameters);
        }
    }
}
 