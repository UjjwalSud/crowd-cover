using CrowdCover.Web.Models.Sharpsports;
using System.ComponentModel.DataAnnotations;

namespace CrowdCover.Web.Models
{
    public class StreamingRoom
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = "Default Description";
        public string Slug { get; set; }
        public bool Active { get; set; } = true;
        public DateTime WhenCreatedUTC { get; set; } = DateTime.UtcNow;

        // Many-to-Many relationship with Event
        public ICollection<StreamingRoomEvent> StreamingRoomEvents { get; set; }

        public ICollection<StreamingRoomBook> StreamingRoomBooks { get; set; }  
    }

    // Join table to support many-to-many relationship
    public class StreamingRoomEvent
    {
        public int StreamingRoomId { get; set; }
        public StreamingRoom StreamingRoom { get; set; }

        public string EventId { get; set; }
        public Event? Event { get; set; }
    }
}
