using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject_T6.Models;

namespace CFProject_T6.Controllers
{
    public class UpdatesController : Controller
    {
        private readonly ProjectContext _context;

        public UpdatesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Updates
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.Updates.Include(u => u.Project);
            return View(await projectContext.ToListAsync());
        }

        // GET: Updates/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updates = await _context.Updates
                .Include(u => u.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updates == null)
            {
                return NotFound();
            }

            return View(updates);
        }

        // GET: Updates/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr");
            return View();
        }

        // POST: Updates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,Descr,Timestamp")] Updates updates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(updates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", updates.ProjectId);
            return View(updates);
        }

        // GET: Updates/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updates = await _context.Updates.FindAsync(id);
            if (updates == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", updates.ProjectId);
            return View(updates);
        }

        // POST: Updates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ProjectId,Descr,Timestamp")] Updates updates)
        {
            if (id != updates.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpdatesExists(updates.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", updates.ProjectId);
            return View(updates);
        }

        // GET: Updates/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updates = await _context.Updates
                .Include(u => u.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updates == null)
            {
                return NotFound();
            }

            return View(updates);
        }

        // POST: Updates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var updates = await _context.Updates.FindAsync(id);
            _context.Updates.Remove(updates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpdatesExists(long id)
        {
            return _context.Updates.Any(e => e.Id == id);
        }
    }
}
