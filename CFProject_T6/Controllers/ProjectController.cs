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
    public class ProjectController : Controller
    {
        private readonly ProjectContext _context;
        private readonly ProjectContext _contextpackage;


        public ProjectController(ProjectContext context)
        {
            _context = context;
            _contextpackage = context;
        }

        // GET: Project
        public IActionResult Index()
        {
            //var projectContext = _context.Projects.Include(p => p.Category).Include(p => p.Creator);
            //return View(await projectContext.ToListAsync());

            return RedirectToAction(nameof(Search));
        }
   
        // GET: Project/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
                return NotFound();

            var projects = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projects == null)
                return NotFound();

            return View(projects);
        }

        // GET: Project/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");

            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectsCreation projects)
        {
            projects.CreatorId = GetUserID();
            projects.Fundsrecv = 0;

            if (ModelState.IsValid)
            {
                _context.Add(projects.Project);
                await _context.SaveChangesAsync();
                
                projects.packages.ProjectId = projects.Project.Id;

                _contextpackage.Add(projects.packages);
                await _contextpackage.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projects.CategoryId);

            return View(projects);

            //_context.Add(projects);
            //await _context.SaveChangesAsync();
            //return Json(new {
            //    title = projects.Title,
            //    redirect = Url.Action("Details", "Project", new { id = projects.Id})
            //});
        }

        // GET: Project/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
                return NotFound();

            projects.CreatorId = GetUserID();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projects.CategoryId);
            return View(projects);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Descr,Goalfunds,CreatorId,Fundsrecv,CategoryId,StartDate,EndDate")] Projects projects)
        {
            if (id != projects.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            projects.CreatorId = GetUserID();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projects.CategoryId);
            return View(projects);
        }

        // GET: Project/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var projects = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projects == null)
                return NotFound();

            return View(projects);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var projects = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(long id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        //GET : Project/Search/5
        public IActionResult Search(long? idd, string title)
        {
            var projectContext = _context.Projects.Include(p => p.Category).Include(p => p.Creator).Where(p => p.Category.Id == idd && p.Title.Contains(title));
            
            if (idd == null && title == null)
                projectContext = _context.Projects.Include(p => p.Category).Include(p => p.Creator);
            else if (idd == null)
                projectContext = _context.Projects.Include(p => p.Category).Include(p => p.Creator).Where(p => p.Title.Contains(title));
            else if (title == null)
                projectContext = _context.Projects.Include(p => p.Category).Include(p => p.Creator).Where(p => p.Category.Id == idd);

            var ProjCat = new ProjectCategory();
            ProjCat.Categories = _context.Categories.ToList();
            ProjCat.Projects = projectContext.ToList();

            if (projectContext == null)
                return NotFound();
            else
                return View(ProjCat);

        }
        private long GetUserID()
        {
            return long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [Authorize]
        public IActionResult MyProject()
        {

            var projectContext = _context.Projects
                                            .Include(p => p.Category)
                                            .Include(p => p.Creator)
                                            .Where(p => p.CreatorId == GetUserID());

            return View(projectContext.ToList());

        }

        [Authorize]
        public IActionResult MyFundedProjects()
        {

            var myBackedContext = _context.BackersProjects.Where(p => p.UserId == GetUserID()).Select(p => p.ProjectId).Distinct().ToList();
            var myFundedProjects = new List<Projects>();
            //IQueryable<Projects> myFundedProjects;
            //var newMyFundedProjects = new List<Projects>();

            foreach (var item in myBackedContext)
            {
                
                myFundedProjects.Add(_context.Projects.Include(p => p.Category).Include(p => p.Creator)
                                                                        .Where(p => p.Id == item).First());
            }
            
            return View(myFundedProjects);

        }
    }
}
