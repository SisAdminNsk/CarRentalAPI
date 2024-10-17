namespace CarRentalAPI.Contracts
{
    public class CarOrderReply
    {
        public CarOrderRequest CarOrderRequest { get; set; }

        public int Status { get; set; }
        public string StatusMessage { get; set; }

        public CarOrderReply(CarOrderRequest carOrderRequest, int status, string statusMessage)
        {
            CarOrderRequest = carOrderRequest;
            Status = status;
            StatusMessage = statusMessage;
        }
    }
}
