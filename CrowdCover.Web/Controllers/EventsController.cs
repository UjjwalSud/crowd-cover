using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports; // Ensure this namespace includes your Event model
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace CrowdCover.Web.Controllers
{
    public class EventsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 500;

        public EventsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<Event>> Get()
        {
            return Ok(_dbContext.Events);
        }

        //[EnableQuery(PageSize = PageSize)]
        //[HttpGet("{key}")]
        //public ActionResult<Event> Get([FromRoute] string key)
        //{
        //    var item = _dbContext.Events.SingleOrDefault(e => e.Id.Equals(key));

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(item);
        //}
    }
}
