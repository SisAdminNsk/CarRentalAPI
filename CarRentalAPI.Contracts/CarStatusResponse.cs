namespace CarRentalAPI.Contracts
{
    public class CarStatusResponse
    {
        public CarStatusResponse() { }
        public Guid? BookerId { get; set; }
        public bool IsFreeForBooking { get; set; }
        public DateTime? DeadlineBooking { get; set;} 
    }
}
