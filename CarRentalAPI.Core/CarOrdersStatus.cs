namespace CarRentalAPI.Core
{
    public static class CarOrdersStatus
    {
        public static string OutOfTime { get; private set; } = "OutOfTime";
        public static string Closed { get; private set; } = "Closed";
        public static string Opened { get; private set; } = "Opened";
        public static string NotConsidered { get; private set; } = "NotConsidered";

        private static HashSet<string> AvailableStatus = new HashSet<string>() 
        { OutOfTime, Closed, Opened, NotConsidered };

        public static bool IsStatusAvailable(string status)
        {
            if (AvailableStatus.Contains(status))
            {
                return true;
            }

            return false;
        }
    }
}
