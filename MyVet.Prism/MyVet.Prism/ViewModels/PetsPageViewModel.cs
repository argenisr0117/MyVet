using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private OwnerResponse _owner;
        private ObservableCollection<PetItemViewModel> _pets;
        private TokenResponse _token;
        private DelegateCommand _addPetCommand;
        private static PetsPageViewModel _instance;

        public PetsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Pets of:";
            LoadPets();
        }

        public static PetsPageViewModel GetInstance()
        {
            return _instance;
        }

        public ObservableCollection<PetItemViewModel> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value); //Se asigna el valor por setproperty para que lo refresque
        }

        public DelegateCommand AddPetCommand => _addPetCommand ?? (_addPetCommand = new DelegateCommand(AddPet));

        //recibir parametros de una pagina a otra
        //public override void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    base.OnNavigatedTo(parameters);

        //    if (parameters.ContainsKey("owner"))
        //    {
        //        _owner = parameters.GetValue<OwnerResponse>("owner");
        //        Pets = new ObservableCollection<PetItemViewModel>(_owner.Pets.Select(p => new PetItemViewModel(_navigationService)
        //        {
        //            Born = p.Born,
        //            Histories = p.Histories,
        //            Id = p.Id,
        //            ImageUrl = p.ImageUrl,
        //            Name = p.Name,
        //            PetType = p.PetType,
        //            Race = p.Race,
        //            Remarks = p.Remarks
        //        }).ToList());
        //    }

        //}

        private void LoadPets()
        {
            //IsRefreshing = true;

            _token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            _owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            Title= $"Pets of: {_owner.FullName}";

            Pets = new ObservableCollection<PetItemViewModel>(_owner.Pets.Select(p => new PetItemViewModel(_navigationService)
            {
                Born = p.Born,
                Histories = p.Histories,
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                PetType = p.PetType,
                Race = p.Race,
                Remarks = p.Remarks
            }).ToList());

            //IsRefreshing = false;
        }

        private async void AddPet()
        {
           await _navigationService.NavigateAsync("AddEditPetPage");
        }

        public async Task UpdateOwnerAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetOwnerByEmailAsync(
                url,
                "/api",
                "/Owners/GetOwnerByEmail",
                "bearer",
                token.Token,
                _owner.Email);

            if (response.IsSuccess)
            {
                var owner = (OwnerResponse)response.Result;
                Settings.Owner = JsonConvert.SerializeObject(owner);
                _owner = owner;
                LoadPets();
            }
        }

    }
}