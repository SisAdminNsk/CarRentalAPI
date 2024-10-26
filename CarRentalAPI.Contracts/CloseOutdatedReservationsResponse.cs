namespace CarRentalAPI.Contracts
{
    public class CloseOutdatedReservationsResponse
    {
        public int TimeToNextRequest { get; set; }

        public Guid NearestWaitingToCloseReservationId { get; set; }

        public bool NoOneRecord { get; set; } = false;

        public CloseOutdatedReservationsResponse(int timeToNextRequest, Guid nearestWaitingToCloseReservationId, bool noOneRecord = false)
        {
            TimeToNextRequest = timeToNextRequest;
            NearestWaitingToCloseReservationId = nearestWaitingToCloseReservationId;
            NoOneRecord = noOneRecord;
        }
    }
}
