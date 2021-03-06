﻿using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyVet.Prism.ViewModels
{
    public class AddEditPetPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private PetResponse _pet;
        private ImageSource _imageSource;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEdit;
        private ObservableCollection<PetTypeResponse> _petTypes;
        private PetTypeResponse _petType;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;

        public AddEditPetPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "New Pet";
            isEnabled = true;
           
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public ObservableCollection<PetTypeResponse> PetTypes
        {
            get => _petTypes;
            set => SetProperty(ref _petTypes, value);
        }

        public PetTypeResponse PetType
        {
            get => _petType;
            set => SetProperty(ref _petType, value);
        }

        public bool isRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool isEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public bool isEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("pet"))
            {
                Pet = parameters.GetValue<PetResponse>("pet");
                if (!string.IsNullOrEmpty(Pet.ImageUrl))
                {
                    ImageSource = Pet.ImageUrl;
                }
                else
                {
                    ImageSource = "noimage";
                }
                isEdit = true;
                Title = "Edit pet";
            }
            else
            {
                Pet = new PetResponse { Born = DateTime.Today };
                ImageSource = "noimage";
                isEdit = false;
                Title = "New pet";
            }

            LoadPetTypesAsync();
        }

        private async void LoadPetTypesAsync()
        {
            isEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnection(url);
            if (!connection)
            {
                isEnabled = true;
                isRunning = false;
                await App.Current.MainPage.DisplayAlert(
                     Languages.Error,
                     Languages.ConnectionError,
                     Languages.Accept);

                await _navigationService.GoBackAsync();
                return;
            }
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetListAsync<PetTypeResponse>(
                url, 
                "/api", 
                "/PetTypes", 
                "bearer", 
                token.Token);

            isEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                     Languages.Error, 
                     response.Message, 
                     Languages.Accept);
                return;
            }

            var petTypes = (List<PetTypeResponse>)response.Result;
            PetTypes = new ObservableCollection<PetTypeResponse>(petTypes);

            if (!string.IsNullOrEmpty(Pet.PetType))
            {
                PetType = PetTypes.FirstOrDefault(pt => pt.Name == Pet.PetType);
            }

        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you want to get the picture from?",
                "Cancel",
                null,
                "Gallery",
                "Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "Camera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void SaveAsync()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            isRunning = true;
            isEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = FilesHelper.ReadFully(_file.GetStream());
            }

            var petRequest = new PetRequest
            {
                Born = Pet.Born,
                Id = Pet.Id,
                ImageArray = imageArray,
                Name = Pet.Name,
                OwnerId = owner.Id,
                PetTypeId = PetType.Id,
                Race = Pet.Race,
                Remarks = Pet.Remarks
            };
            var connection = await _apiService.CheckConnection(url);
            if (!connection)
            {
                isEnabled = true;
                isRunning = false;
                await App.Current.MainPage.DisplayAlert(
                     Languages.Error,
                     Languages.ConnectionError,
                     Languages.Accept);
                return;
            }
            Response<object> response;

            if (isEdit)
            {
                response = await _apiService.PutAsync(url, "/api", "/Pets", petRequest.Id, petRequest, "bearer", token.Token);
            }
            else
            {
                response = await _apiService.PostAsync(url, "/api", "/Pets", petRequest, "bearer", token.Token);
            }

            if (!response.IsSuccess)
            {
                isRunning = false;
                isEnabled = true;
                await App.Current.MainPage.DisplayAlert( Languages.Error, response.Message,  Languages.Accept);
                return;
            }

            await PetsPageViewModel.GetInstance().UpdateOwnerAsync();

            isRunning = false;
            isEnabled = true;

            await App.Current.MainPage.DisplayAlert(
                 "Ok",
                 string.Format(Languages.CreateEditPetConfirm, isEdit ? Languages.Edited : Languages.Created),
                 Languages.Accept);

            await _navigationService.GoBackToRootAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Pet.Name))
            {
                await App.Current.MainPage.DisplayAlert( Languages.Error, "You must enter a name",  Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Pet.Race))
            {
                await App.Current.MainPage.DisplayAlert( Languages.Error, "You must enter a race",  Languages.Accept);
                return false;
            }

            if (PetType == null)
            {
                await App.Current.MainPage.DisplayAlert( Languages.Error, "You must select a pet type",  Languages.Accept);
                return false;
            }

            return true;
        }


    }
}

