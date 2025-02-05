namespace CrowdCover.Web.Models.ViewModels
{
    public class RemoveEventViewModel
    {
        public int StreamingRoomId { get; set; }  // ID of the StreamingRoom from which the event is being removed
        public string EventId { get; set; }  // ID of the event that is being removed
        public string EventName { get; set; }  // Name of the event being removed (for display purposes)
    }
}
