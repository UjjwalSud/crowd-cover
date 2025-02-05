using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace CrowdCover.Web.Controllers
{
    public class DynamicDataController : ODataController
    {
        //context
        ApplicationDbContext _db;
        const int pageSize = 500;
        public DynamicDataController(ApplicationDbContext db)
        {
            _db = db;
        }

        [EnableQuery(PageSize = pageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<DynamicDataVariable>> Get()
        {
            return Ok(_db.DynamicDataVariables);
        }

        [EnableQuery(PageSize = pageSize)]
        [HttpGet]
        public ActionResult<DynamicDataVariable> Get([FromRoute] int key)
        {
            var item = _db.DynamicDataVariables.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
