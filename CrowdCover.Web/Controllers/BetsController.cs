using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace CrowdCover.Web.Controllers
{
    public class BetsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 500;

        public BetsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Bets
        [EnableQuery(PageSize = PageSize)]
        [HttpGet]
        public ActionResult<IEnumerable<Bet>> Get()
        {
            return Ok(_dbContext.Bets);
        }

        //// GET: /Bets/{key}
        //[EnableQuery(PageSize = PageSize)]
        //[HttpGet("{key}")]
        //public ActionResult<Bet> Get([FromRoute] string key)
        //{
        //    var bet = _dbContext.Bets.SingleOrDefault(b => b.Id.Equals(key));

        //    if (bet == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(bet);
        //}

        //// POST: /Bets
        //[HttpPost]
        //public ActionResult<Bet> Post([FromBody] Bet bet)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _dbContext.Bets.Add(bet);
        //    _dbContext.SaveChanges();

        //    return Created(bet);
        //}

        //// PUT: /Bets/{key}
        //[HttpPut("{key}")]
        //public IActionResult Put([FromRoute] string key, [FromBody] Bet update)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var existingBet = _dbContext.Bets.SingleOrDefault(b => b.Id.Equals(key));

        //    if (existingBet == null)
        //    {
        //        return NotFound();
        //    }

        //    _dbContext.Entry(existingBet).CurrentValues.SetValues(update);
        //    _dbContext.SaveChanges();

        //    return NoContent();
        //}

        //// DELETE: /Bets/{key}
        //[HttpDelete("{key}")]
        //public IActionResult Delete([FromRoute] string key)
        //{
        //    var bet = _dbContext.Bets.SingleOrDefault(b => b.Id.Equals(key));

        //    if (bet == null)
        //    {
        //        return NotFound();
        //    }

        //    _dbContext.Bets.Remove(bet);
        //    _dbContext.SaveChanges();

        //    return NoContent();
        //}
    }
}
