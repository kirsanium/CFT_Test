using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Task = CFT.Models.Task;

namespace CFT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        
        private readonly ProjectContext db;

        public ProjectsController(ProjectContext context)
        {
            db = context;
            db.Database.Migrate();
        }
        
        // GET api/projects?page=1
        [HttpGet]
        public ActionResult<IEnumerable<Project>> Get([FromQuery] int? page)
        {
            if (page == null)
                return Redirect("~/api/projects?page=1");

            var count = 10;
            var index = (int)page - 1;
            var projectsList = db.Projects.ToList();

            int elemCount;

            if (projectsList.Count - index * count < count)
                elemCount = projectsList.Count - index * count;
            else elemCount = count;

            if (elemCount <= 0)
                return NotFound();
            
            return projectsList.GetRange(index * count, elemCount);
        }

        // GET api/projects/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
        {
            var project = (Project)db.Find(typeof(Project), id);
            if (project == null)
                return NotFound();
            else
                return project;
        }
    
        // POST api/projects
        [HttpPost]
        public ActionResult<Project> Post([FromBody] Project postedProject)
        {
            db.Add(postedProject);
            db.SaveChanges();
            return postedProject;
        }

        // PUT api/projects/5
        [HttpPut("{id}")]
        public ActionResult<Project> Put(int id, [FromBody] PutProject putProject)
        {
            var project = (Project)db.Find(typeof(Project), id);
            if (project == null)
                return NotFound();
            db.Update(project);
            project.UpdateWith(putProject);
            db.SaveChanges();
            return project;
        }

        // DELETE api/projects/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var project = (Project)db.Find(typeof(Project), id);
            if (project == null)
                return NotFound();
            db.Remove(project);
            db.SaveChanges();
            return Ok();
        }
        
        // GET api/projects/5/tasks
        [HttpGet("{id}/tasks")]
        public ActionResult<IEnumerable<Task>> GetTasks(int id, [FromQuery] int? page, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string status, [FromQuery] int? priority, [FromQuery] string dateSort, [FromQuery] string prioritySort)
        {
            var url = new StringBuilder().Append("~/api/tasks?page=");
            if (page == null)
                page = 1;
            url.Append(page);
            url.Append("&projectid=")
               .Append(id);
            if (startDate != null)
                url.Append("&startdate=")
                   .Append(startDate);
            if (endDate != null)
                url.Append("&enddate=")
                   .Append(endDate);
            if (status != null)
                url.Append("&status=")
                   .Append(status);
            if (priority != null)
                url.Append("&priority=")
                   .Append(priority);
            if (dateSort != null)
                url.Append("&datesort=")
                   .Append(dateSort);
            if (prioritySort != null)
                url.Append("&prioritysort=")
                   .Append(prioritySort);
            return Redirect(url.ToString());
        }
    }
}