using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MyVet.Prism.ViewModels
{
    public class RecoverPasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _recoverCommand;

        public RecoverPasswordPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Recover password";
            isEnabled = true;
        }

        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand = new DelegateCommand(Recover));

        public string Email { get; set; }

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

        private async void Recover()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            isRunning = true;
            isEnabled = false;

            var request = new EmailRequest
            {
                Email = Email
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.RecoverPasswordAsync(
                url,
                "/api",
                "/Account/RecoverPassword",
                request);

            isRunning = false;
            isEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                   "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await App.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                "Accept");
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a valid email"
                    , "Accept");
                return false;
            }

            return true;
        }
    }
}