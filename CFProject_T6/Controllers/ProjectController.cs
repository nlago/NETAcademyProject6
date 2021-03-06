﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFProject_T6.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CFProject_T6.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProjectController(ProjectContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

            var project = await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
                return NotFound();

            var iscreator = _context.Projects.Any(p => p.Id == id && p.CreatorId == GetUserID());
            var photoUrl = _context.Photos.Where(p => p.ProjectId == id).Select(p => p.Filename).First();

            var UIProjectDetails = new ProjectSearchResultVM();

           UIProjectDetails.Photo = new Photos();


            UIProjectDetails.IsCreator = iscreator;
            UIProjectDetails.Project = project;

            UIProjectDetails.Photo.Filename = Path.Combine(_hostingEnvironment.WebRootPath, photoUrl);

            return View(UIProjectDetails);
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
        public async Task<IActionResult> Create(ProjectsCreation projectVM)
        {
            if ((projectVM.Project.StartDate <= projectVM.Project.EndDate) && (projectVM.Project.EndDate > DateTime.UtcNow.Date))
            {

                projectVM.Project.CreatorId = GetUserID();
                projectVM.Project.Fundsrecv = 0;
                projectVM.Project.Packages = new List<Packages>
                {
                    projectVM.Packages
                };

                var path = $"/uploads/{projectVM.Photo.FileName}";
                var pathForHost = _hostingEnvironment.WebRootPath + $"/uploads/{projectVM.Photo.FileName}";

                using (var stream = new FileStream(pathForHost, FileMode.Create))
                {
                    await projectVM.Photo.CopyToAsync(stream);
                }

                var myphoto = new Photos()
                {
                    Filename = path
                };

                projectVM.Project.Photos = new List<Photos>
                {
                    myphoto
                };

                if (ModelState.IsValid)
                {
                    _context.Add(projectVM.Project);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectVM.Project.CategoryId);
            ViewData["Wrong Date"] = "Check your Start Date and your End Date";

            return View(projectVM);

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
            var projectCreator = _context.Projects.Where(p => p.Id == id).Select(p => p.CreatorId).First();

            if (GetUserID() == projectCreator)
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
            else
            {
                return RedirectToAction(nameof(Search));
            }
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Projects projects)
        {
            if (id != projects.Id)
                return NotFound();

            if ((projects.StartDate <= projects.EndDate) && (projects.EndDate > DateTime.UtcNow.Date))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        projects.CreatorId = GetUserID();
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
                //projects.CreatorId = GetUserID();  
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projects.CategoryId);
            ViewData["Wrong Date"] = "Check your Start Date and your End Date";
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

            var packages = _context.Packages.Where(p => p.ProjectId == id).ToList();
            var photo = _context.Photos.Where(p => p.ProjectId == id).ToList();
            var backetproject = _context.BackersProjects.Where(b => b.ProjectId == id).ToList();

            var delete = new Delete();
            delete.Project = projects;
            delete.Photos = photo;
            delete.Packages = packages;
            delete.BackersProjects = backetproject;


            if (projects == null)
                return NotFound();

            return View(delete);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {

            var backer = _context.BackersProjects.Where(p => p.ProjectId == id).ToList();
            foreach (var item in backer)
                _context.BackersProjects.Remove(item);

            var photos = _context.Photos.Where(p => p.ProjectId == id).ToList();
            foreach(var item in photos )
            _context.Photos.Remove(item);


            var packages = _context.Packages.Where(p => p.ProjectId == id).ToList();
            foreach (var item in packages)
                _context.Packages.Remove(item);

            

            


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
            //ProjCat.Projects = projectContext.ToList();

            var allPhotos = _context.Photos;
            var UIProjectList = projectContext.Select(p => new ProjectSearchResultVM
            {
                Project = p,
                Photo = allPhotos.FirstOrDefault(photo => photo.ProjectId == p.Id) 
            });

            ProjCat.Projects = UIProjectList.ToList();

            //ProjCat.Photos = _context.Photos.Where(p => projectContext.Contains(p.Project)).ToList();

            if (projectContext == null)
                return NotFound();
            else
                return View(ProjCat);

        }
        private long GetUserID()
        {
            try
            {
                return long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch(ArgumentNullException e)
            {
                return 0;
            }
        }

        [Authorize]
        public IActionResult MyProjects()
        {
            var projectContext = _context.Projects
                                            .Include(p => p.Category)
                                            .Include(p => p.Creator)
                                            .Where(p => p.CreatorId == GetUserID());

            var myProjects = new ProjectCategory();
            //ProjCat.Categories = _context.Categories.ToList();
            //ProjCat.Projects = projectContext.ToList();

            var allPhotos = _context.Photos;
            var UIProjectList = projectContext.Select(p => new ProjectSearchResultVM
            {
                Project = p,
                Photo = allPhotos.FirstOrDefault(photo => photo.ProjectId == p.Id)
            });

            myProjects.Projects = UIProjectList.ToList();

            return View(myProjects);

        }

        [Authorize]
        public IActionResult MyFundedProjects()
        {
            var myBackedContext = _context.BackersProjects.Where(p => p.UserId == GetUserID()).Select(p => p.ProjectId).Distinct().ToList();
            var myFundedProjectsContext = new List<Projects>();

            foreach (var item in myBackedContext)
            {
                myFundedProjectsContext.Add(_context.Projects.Include(p => p.Category).Include(p => p.Creator)
                                                                        .Where(p => p.Id == item).SingleOrDefault());
            }

           
                var myFundedProjects = new ProjectCategory();
                var allPhotos = _context.Photos;
                var UIProjectList = myFundedProjectsContext.Select(p => new ProjectSearchResultVM
                {
                    Project = p,
                    Photo = allPhotos.FirstOrDefault(photo => photo.ProjectId == p.Id)
                });

                myFundedProjects.Projects = UIProjectList.ToList();

                return View(myFundedProjects);
            

        }
    }
}
