using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;  // Replace with the actual namespace for Bettor
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace CrowdCover.Web.Controllers
{
    public class BettorsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 500;

        public BettorsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<Bettor>> Get()
        {
            return Ok(_dbContext.Bettors);
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<Bettor> Get([FromRoute] string key)
        {
            var item = _dbContext.Bettors.SingleOrDefault(b => b.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Bettor> Post([FromBody] Bettor bettor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Bettors.Add(bettor);
            _dbContext.SaveChanges();

            return Created(bettor);
        }

        [HttpPut]
        public IActionResult Put([FromRoute] string key, [FromBody] Bettor update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBettor = _dbContext.Bettors.SingleOrDefault(b => b.Id.Equals(key));

            if (existingBettor == null)
            {
                return NotFound();
            }

            _dbContext.Entry(existingBettor).CurrentValues.SetValues(update);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] string key)
        {
            var bettor = _dbContext.Bettors.SingleOrDefault(b => b.Id.Equals(key));

            if (bettor == null)
            {
                return NotFound();
            }

            _dbContext.Bettors.Remove(bettor);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
