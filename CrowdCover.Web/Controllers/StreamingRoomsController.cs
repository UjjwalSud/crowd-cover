using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace CrowdCover.Web.Controllers
{
    public class StreamingRoomsController : ODataController
    {
        //context
        ApplicationDbContext _db;
        const int pageSize = 500;
        public StreamingRoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [EnableQuery(PageSize = pageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<StreamingRoom>> Get()
        {
            return Ok(_db.StreamingRooms);
        }

        [EnableQuery(PageSize = pageSize)]
        [HttpGet]
        public ActionResult<StreamingRoom> Get([FromRoute] int key)
        {
            var item = _db.StreamingRooms.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
