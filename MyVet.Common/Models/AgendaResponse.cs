using System;

namespace MyVet.Common.Models
{
    public class AgendaResponse
    {
        public DateTime Date { get; set; }
        public DateTime DateLocal => Date.ToLocalTime();
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public OwnerResponse Owner { get; set; }

        public PetResponse Pet { get; set; }

        public string Remarks { get; set; }
    }
}