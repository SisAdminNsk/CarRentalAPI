namespace CarRentalAPI.Contracts
{
    public class GetServerTimeResponse
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int Hours { get; set; }

        public GetServerTimeResponse(int day, int month, int year, int hours)
        {
            Day = day;
            Month = month;
            Year = year;
            Hours = hours;
        }
    }
}
