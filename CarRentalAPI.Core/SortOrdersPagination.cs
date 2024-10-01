namespace CarRentalAPI.Core
{
    public static class SortOrdersPagination
    {
        private static HashSet<string> AvailableOrders = new HashSet<string>()
        { "ascending", "descending" };

        public static string Ascending { get; private set; } = "ascending";
        public static string Descending { get; private set; } = "descending";
        public static bool IsSortOrderAvailable(string sortOrder)
        {
            if (AvailableOrders.Contains(sortOrder))
            {
                return true;
            }

            return false;
        }
    }
}
