using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = _dataContext.Owners.Select(owner => new SelectListItem
            {
                Text = owner.User.FullNameWithDocument,
                Value = $"{owner.Id.ToString()}"
            })
               .OrderBy(owner => owner.Text)
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select an owner]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboPets(int ownerId)
        {
            var list = _dataContext.Pets.Where(p => p.Owner.Id == ownerId).Select(pet => new SelectListItem
            {
                Text = pet.Name,
                Value = $"{pet.Id.ToString()}"
            })
              .OrderBy(pet => pet.Text)
              .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a pet]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboPetTypes()
        {
            //var list = new List<SelectListItem>();
            //foreach (var petType in _dataContext.PetTypes)
            //{
            //    list.Add(new SelectListItem
            //    {
            //        Text = petType.Name,
            //        Value = $"{petType.Id}"
            //    });
            //}

            var list = _dataContext.PetTypes.Select(petType => new SelectListItem
            {
                Text = petType.Name,
                Value = $"{petType.Id}"
            })
                .OrderBy(petType => petType.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a pet type]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
           var list = _dataContext.ServiceTypes.Select(serviceType => new SelectListItem
            {
                Text = serviceType.Name,
                Value = $"{serviceType.Id}"
            })
                .OrderBy(serviceType => serviceType.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a service type]",
                Value = "0"
            });
            return list;
        }
    }
}
