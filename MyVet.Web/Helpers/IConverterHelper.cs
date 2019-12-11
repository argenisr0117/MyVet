using MyVet.Common.Models;
using MyVet.Web.Data.Entities;
using MyVet.Web.Models;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew);

        HistoryViewModel ToHistoryViewModel(History history);

        OwnerResponse ToOwnerResposne(Owner owner);

        Task<Pet> ToPetAsync(PetViewModel model, string path, bool isNew);

        PetResponse ToPetResponse(Pet pet);

        PetViewModel ToPetViewModel(Pet pet);
    }
}