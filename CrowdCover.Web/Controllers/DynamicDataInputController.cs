using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace CrowdCover.Web.Controllers
{
    [Authorize]
    public class DynamicDataInputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DynamicDataInputController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DynamicDataVariables
        public async Task<IActionResult> Index()
        {
            return View(await _context.DynamicDataVariables.ToListAsync());
        }

        // GET: DynamicDataVariables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicDataVariable = await _context.DynamicDataVariables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dynamicDataVariable == null)
            {
                return NotFound();
            }

            return View(dynamicDataVariable);
        }

        // GET: DynamicDataVariables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DynamicDataVariables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value")] DynamicDataVariable dynamicDataVariable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dynamicDataVariable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dynamicDataVariable);
        }

        // GET: DynamicDataVariables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicDataVariable = await _context.DynamicDataVariables.FindAsync(id);
            if (dynamicDataVariable == null)
            {
                return NotFound();
            }
            return View(dynamicDataVariable);
        }

        // POST: DynamicDataVariables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] DynamicDataVariable dynamicDataVariable)
        {
            if (id != dynamicDataVariable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dynamicDataVariable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DynamicDataVariableExists(dynamicDataVariable.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dynamicDataVariable);
        }

        // GET: DynamicDataVariables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicDataVariable = await _context.DynamicDataVariables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dynamicDataVariable == null)
            {
                return NotFound();
            }

            return View(dynamicDataVariable);
        }

        // POST: DynamicDataVariables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dynamicDataVariable = await _context.DynamicDataVariables.FindAsync(id);
            if (dynamicDataVariable != null)
            {
                _context.DynamicDataVariables.Remove(dynamicDataVariable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DynamicDataVariableExists(int id)
        {
            return _context.DynamicDataVariables.Any(e => e.Id == id);
        }
    }
}
