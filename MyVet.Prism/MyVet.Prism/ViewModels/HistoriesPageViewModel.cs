using MyVet.Common.Helpers;
using MyVet.Common.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
    public class HistoriesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<HistoryItemViewModel> _histories;
        private PetResponse _pet;

        public HistoriesPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Histories";
            _navigationService = navigationService;
            Pet = JsonConvert.DeserializeObject<PetResponse>(Settings.Pet);
            LoadHistories();
        }

        public ObservableCollection<HistoryItemViewModel> Histories
        {
            get => _histories;
            set => SetProperty(ref _histories, value);
        }

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        //public override void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    base.OnNavigatedTo(parameters);

        //    if (parameters.ContainsKey("pet"))
        //    {
        //        Pet = parameters.GetValue<PetResponse>("pet");
        //        Title = $"Histories of: {Pet.Name }";
        //        LoadHistories();
        //        Histories = new ObservableCollection<HistoryItemViewModel>(Pet.Histories.Select(h => new HistoryItemViewModel(_navigationService)
        //        {
        //            Date = h.Date,
        //            Description = h.Description,
        //            Id = h.Id,
        //            Remarks = h.Remarks,
        //            ServiceType = h.ServiceType
        //        }).ToList());
        //    }
        //}

        private void LoadHistories()
        {
            Histories = new ObservableCollection<HistoryItemViewModel>(Pet.Histories.Select(h => new HistoryItemViewModel(_navigationService)
            {
                Date = h.Date,
                Description = h.Description,
                Id = h.Id,
                Remarks = h.Remarks,
                ServiceType = h.ServiceType
            }).ToList());
        }
    }
}