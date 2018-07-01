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
using Microsoft.AspNetCore.Identity;


namespace CFProject_T6.Controllers
{
    public class BackersProjectsController : Controller
    {
        private readonly ProjectContext _context;

        public BackersProjectsController(ProjectContext context)
        {
            _context = context;
        }              

        [Authorize]
        [HttpGet("project/{project_id:long}/packages/{package_id}/purchase", Name = "purchase")]
        public async Task<IActionResult> Create(long package_id, long project_id)
        {
            var selected_project_exists = _context.Projects.Any(p => p.Id == project_id);
            var selected_package_exists = _context.Packages.Any(p => p.Id == package_id);
            if (!selected_project_exists || !selected_package_exists)
            {
                return RedirectToAction("Index", "Project");
            }

            //initialize required vars
            long userid = GetUserID();
            var live = false;

            var selected_project_startdate = await _context.Projects
                                .Where(p => p.Id == project_id)
                                .Select(p => p.StartDate)
                                .SingleOrDefaultAsync();

            //if project is live, start building a purchase confirmation model with all purchase details...
            if (selected_project_startdate < DateTime.Now)
            {
                live = true;

                //fetch additional data from DB...
                var username = await _context.Users
                    .Where(u => u.Id == userid)
                    .Select(u => u.UserName)
                    .SingleOrDefaultAsync();
                var projecttitle = await _context.Projects
                    .Where(p => p.Id == project_id)
                    .Select(p => p.Title)
                    .SingleOrDefaultAsync();
                var reward = await _context.Packages
                    .Where(p => p.Id == package_id)
                    .Select(p => p.Reward)
                    .SingleOrDefaultAsync();
                var amount = await _context.Packages
                    .Where(p => p.Id == package_id)
                    .Select(p => p.DonationUpperlim)
                    .SingleOrDefaultAsync();

                PurchaseConfirmationModel successful_purchase = new PurchaseConfirmationModel
                {
                    UserId = userid,
                    PackageId = package_id,
                    ProjectId = project_id,
                    Username = username,
                    ProjectTitle = projecttitle,
                    Reward = reward,
                    Amount = amount,
                    ProjectIsLive = live
                };
                return View(successful_purchase);
            }
            
            //else build an invalid purchase model...
            PurchaseConfirmationModel invalid_purchase = new PurchaseConfirmationModel
            {                
                ProjectIsLive = live
            };

            return View(invalid_purchase);

        }

        [Authorize]
        [HttpPost("project/{project_id:long}/packages/{package_id}/purchase", Name = "purchase")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (BackersProjects backersProjects, long project_id, long package_id)
        {

            //Protect from POSTing a donation for a project which is not LIVE...
            var selected_project_startdate = await _context.Projects
                    .Where(p => p.Id == project_id)
                    .Select(p => p.StartDate)
                    .SingleOrDefaultAsync();

            if (selected_project_startdate > DateTime.Now)
            {
                return RedirectToAction("Index", "Packages");
            }
            //<---------- protection code ends here ---------->

            backersProjects.UserId = GetUserID();
            backersProjects.ProjectId = project_id;
            backersProjects.PackageId = package_id;

            if (ModelState.IsValid)
            {
                _context.Add(backersProjects);
                await _context.SaveChangesAsync();

                //Update project's financial progress here...

                //Grab the project & donation amount from DB
                var target_project = await _context.Projects.FindAsync(project_id);
                var selected_package = await _context.Packages.FindAsync(package_id);
                var donation_amount = selected_package.DonationUpperlim;

                //Add the new donation amount to the particular project...
                target_project.Fundsrecv += donation_amount;

                //Update the selected project...
                _context.Update(target_project);
                await _context.SaveChangesAsync();

                var Project = _context.Projects.Single(p => p.Id == project_id);
                var Users = _context.BackersProjects
                .Where(b => b.ProjectId == Project.Id)
                .Select(b => b.UserId)
                .ToList();
                var UserList = new List<Users>();
                foreach(var user in Users)
                {
                    UserList.Add(_context.Users.Single(u => u.Id == user));
                }
                var User = _context.Users.Single(u => u.Id == Project.CreatorId);
                CheckGoal.Getmails(Project, UserList ,User);
                

                return RedirectToAction("Index", "Packages");
            }

            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Reward", backersProjects.PackageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Descr", backersProjects.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", backersProjects.UserId);
            return View(backersProjects);
        }

        //private bool BackersProjectsExists(long id)
        //{
        //    return _context.BackersProjects.Any(e => e.Id == id);
        //}

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
