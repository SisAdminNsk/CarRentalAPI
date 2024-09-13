namespace CarRentalAPI.Contracts
{
    public class EmailMessageResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public EmailMessageResponse(int statusCode, string message) 
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
