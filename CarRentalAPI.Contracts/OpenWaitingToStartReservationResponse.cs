namespace CarRentalAPI.Contracts
{
    public class OpenWaitingToStartReservationsResponse
    {
        public int TimeToNextRequest { get; set; }
        public Guid NearestWaitingToStartReservationId { get; set; }
        public bool NoOneRecord { get; set; } = false;

        public OpenWaitingToStartReservationsResponse(int timeToNextRequest, Guid nearestWaitingToStartReservationId, bool noOneRecord = false)
        {
            TimeToNextRequest = timeToNextRequest;
            NearestWaitingToStartReservationId = nearestWaitingToStartReservationId;
            NoOneRecord = noOneRecord;
        }
    }
}
