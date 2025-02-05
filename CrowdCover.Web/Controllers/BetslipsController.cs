using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports; // Ensure this namespace includes your BetSlip model
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace CrowdCover.Web.Controllers
{
    public class BetSlipsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 500;

        public BetSlipsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<BetSlip>> Get()
        {
            return Ok(_dbContext.BetSlips);
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet("{key}")]
        public ActionResult<BetSlip> Get([FromRoute] string key)
        {
            var item = _dbContext.BetSlips.SingleOrDefault(b => b.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<BetSlip> Post([FromBody] BetSlip betSlip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.BetSlips.Add(betSlip);
            _dbContext.SaveChanges();

            return Created(betSlip);
        }

        [HttpPut("{key}")]
        public IActionResult Put([FromRoute] string key, [FromBody] BetSlip update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBetSlip = _dbContext.BetSlips.SingleOrDefault(b => b.Id.Equals(key));

            if (existingBetSlip == null)
            {
                return NotFound();
            }

            _dbContext.Entry(existingBetSlip).CurrentValues.SetValues(update);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{key}")]
        public IActionResult Delete([FromRoute] string key)
        {
            var betSlip = _dbContext.BetSlips.SingleOrDefault(b => b.Id.Equals(key));

            if (betSlip == null)
            {
                return NotFound();
            }

            _dbContext.BetSlips.Remove(betSlip);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
