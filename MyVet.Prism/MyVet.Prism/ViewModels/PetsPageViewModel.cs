﻿using MyVet.Common.Helpers;
using MyVet.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private OwnerResponse _owner;
        private ObservableCollection<PetItemViewModel> _pets;
        private TokenResponse _token;
        private DelegateCommand _addPetCommand;
        public PetsPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Pets of:";
            _navigationService = navigationService;
            LoadPets();
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
    }
}