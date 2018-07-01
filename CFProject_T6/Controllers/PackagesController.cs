using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject_T6.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CFProject_T6.Controllers
{
    public class PackagesController : Controller
    {
        private readonly ProjectContext _context;

        public PackagesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Packages
        [HttpGet("Project/{project_id:long}/Packages")]
        public async Task<IActionResult> Index(long project_id)
        {
            var projectContext = _context.Packages.Include(p => p.Project);
            var packages = await projectContext.Where(t => t.ProjectId == project_id).ToListAsync();
            var iscreator = _context.Projects.Any(p => p.Id == project_id && p.CreatorId == GetUserID());

            PackagesVM packages_model = new PackagesVM { Packages = packages, IsCreator = iscreator };

            return View(packages_model);
        }        

        // GET: Packages/Create
        [Authorize]
        public IActionResult Create()
        {
            var user_projects = _context.Projects.Where(p => p.CreatorId == GetUserID());
            ViewData["ProjectId"] = new SelectList(user_projects, "Id", "Title");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DonationUpperlim,Reward,ProjectId")] Packages packages, long project_id)
        {
            if (ModelState.IsValid)
            {
                _context.Add(packages);
                await _context.SaveChangesAsync();

                //we need this to navigate back to the project we were browsing...
                project_id = packages.ProjectId;

                return RedirectToAction(nameof(Index), new { project_id });
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", packages.ProjectId);
            return View(packages);
        }

        private bool PackagesExists(long id)
        {
            return _context.Packages.Any(e => e.Id == id);
        }

        private long GetUserID()
        {
            try
            {
                return long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (ArgumentNullException e)
            {
                return 0;
            }
        }
    }
}
