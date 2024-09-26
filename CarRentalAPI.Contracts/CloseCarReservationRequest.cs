using CarRentalAPI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class CloseCarReservationRequest
    {
        public Guid OpenedCarOrderId { get; set; }
        [Required]
        [CarOrderStatusValidation]
        public string Status { get; set; }
        public CloseCarReservationRequest(Guid openedCarOrderId, string status) 
        {
            OpenedCarOrderId = openedCarOrderId;
            Status = status;
        }
    }
}
