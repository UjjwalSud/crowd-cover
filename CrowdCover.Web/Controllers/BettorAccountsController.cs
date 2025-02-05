using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;  // Ensure this namespace includes your BettorAccount model
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace CrowdCover.Web.Controllers
{
    public class BettorAccountsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 500;

        public BettorAccountsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<BettorAccount>> Get()
        {
            return Ok(_dbContext.BettorAccounts);
        }

        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<BettorAccount> Get([FromRoute] string key)
        {
            var item = _dbContext.BettorAccounts.SingleOrDefault(b => b.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<BettorAccount> Post([FromBody] BettorAccount bettorAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.BettorAccounts.Add(bettorAccount);
            _dbContext.SaveChanges();

            return Created(bettorAccount);
        }

        [HttpPut]
        public IActionResult Put([FromRoute] string key, [FromBody] BettorAccount update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBettorAccount = _dbContext.BettorAccounts.SingleOrDefault(b => b.Id.Equals(key));

            if (existingBettorAccount == null)
            {
                return NotFound();
            }

            _dbContext.Entry(existingBettorAccount).CurrentValues.SetValues(update);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] string key)
        {
            var bettorAccount = _dbContext.BettorAccounts.SingleOrDefault(b => b.Id.Equals(key));

            if (bettorAccount == null)
            {
                return NotFound();
            }

            _dbContext.BettorAccounts.Remove(bettorAccount);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
