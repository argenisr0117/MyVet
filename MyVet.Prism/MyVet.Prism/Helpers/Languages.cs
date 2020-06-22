using MyVet.Prism.Interfaces;
using MyVet.Prism.Resources;
using Xamarin.Forms;

namespace MyVet.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;

        public static string ButtomRegister => Resource.ButtomRegister;
        public static string ConnectionError => Resource.ConnectionError;
        public static string EmailError => Resource.EmailError;
        public static string EmailPlaceholder => Resource.EmailPlaceholder;
        public static string Error => Resource.Error;
        public static string ForgotPassword => Resource.ForgotPassword;

        public static string LoadingIndicator => Resource.LoadingIndicator;
        public static string Login => Resource.Login;

        public static string LoginError => Resource.LoginError;

        public static string Password => Resource.Password;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordPlaceholder => Resource.PasswordPlaceholder;
        public static string ProblemError => Resource.ProblemError;
        public static string Rememberme => Resource.Rememberme;
        public static string CreateEditPetConfirm => Resource.CreateEditPetConfirm;
        public static string Created => Resource.Created;
        public static string Edited => Resource.Edited;

    }
}