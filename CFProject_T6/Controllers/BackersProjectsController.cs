using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject_T6.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CFProject_T6.Controllers
{
    public class BackersProjectsController : Controller
    {
        private readonly ProjectContext _context;

        public BackersProjectsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: BackersProjects
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.BackersProjects.Include(b => b.Package).Include(b => b.Project).Include(b => b.User);
            return View(await projectContext.ToListAsync());
        }

        // GET: BackersProjects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backersProjects = await _context.BackersProjects
                .Include(b => b.Package)
                .Include(b => b.Project)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (backersProjects == null)
            {
                return NotFound();
            }

            return View(backersProjects);
        }

        [Authorize]
        // GET: BackersProjects/Create
        [HttpGet("project/{project_id:long}/packages/details/{package_id}/purchase", Name = "purchase")]
        public IActionResult Create()
        {
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Reward");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: BackersProjects/Create
        [Authorize]
        [HttpPost("project/{project_id:long}/packages/details/{package_id}/purchase", Name = "pur")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (BackersProjects backersProjects, long project_id, long package_id)
        {

            backersProjects.UserId = GetUserID();
            backersProjects.ProjectId = project_id;
            backersProjects.PackageId = package_id;

            if (ModelState.IsValid)
            {
                _context.Add(backersProjects);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Reward", backersProjects.PackageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", backersProjects.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", backersProjects.UserId);
            return View(backersProjects);
        }

        // GET: BackersProjects/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backersProjects = await _context.BackersProjects.FindAsync(id);
            if (backersProjects == null)
            {
                return NotFound();
            }
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Reward", backersProjects.PackageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", backersProjects.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", backersProjects.UserId);
            return View(backersProjects);
        }

        // POST: BackersProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,ProjectId,PackageId")] BackersProjects backersProjects)
        {
            if (id != backersProjects.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(backersProjects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BackersProjectsExists(backersProjects.Id))
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
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Reward", backersProjects.PackageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", backersProjects.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", backersProjects.UserId);
            return View(backersProjects);
        }

        // GET: BackersProjects/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var backersProjects = await _context.BackersProjects
                .Include(b => b.Package)
                .Include(b => b.Project)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (backersProjects == null)
            {
                return NotFound();
            }

            return View(backersProjects);
        }

        // POST: BackersProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var backersProjects = await _context.BackersProjects.FindAsync(id);
            _context.BackersProjects.Remove(backersProjects);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BackersProjectsExists(long id)
        {
            return _context.BackersProjects.Any(e => e.Id == id);
        }

        private long GetUserID()
        {
            return long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
