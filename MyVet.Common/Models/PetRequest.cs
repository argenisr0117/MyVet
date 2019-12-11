using System;
using System.ComponentModel.DataAnnotations;

namespace MyVet.Common.Models
{
    public class PetRequest
    {
        [Required]
        public DateTime Born { get; set; }

        public int Id { get; set; }

        public byte[] ImageArray { get; set; }

        [Required]
        public string Name { get; set; }

        public int OwnerId { get; set; }
        public int PetTypeId { get; set; }
        public string Race { get; set; }
        public string Remarks { get; set; }
    }
}