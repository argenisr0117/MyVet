using System;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region PrivateAttrbs
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand; 
        #endregion

        public LoginPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
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
            await App.Current.MainPage.DisplayAlert("Ok", "Fuckkkk yeaaaah.", "Accept");
        }
    }
}
 