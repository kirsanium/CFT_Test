using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Task = CFT.Models.Task;
using TaskStatus = CFT.Models.TaskStatus;

namespace CFT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        enum SortType
        {
            Priority,
            Date
        }
        
        private readonly ProjectContext db;

        public TasksController(ProjectContext context)
        {
            db = context;
            db.Database.Migrate();
        }
        
        // GET api/tasks?page=1&parameter=value
        // Query parameters:
        // startdate: date in ddMMyyyy format
        // enddate: date in ddMMyyyy format
        // status: [new|inprogress|closed]
        // priority: 0 to 255
        // datesort: asc for ascending sort, desc for descending
        // prioritysort: asc for ascending sort, desc for descending
        // You can't sort by both parameters simultaneously.
        [HttpGet]
        public ActionResult<IEnumerable<Task>> Get([FromQuery] int? page, [FromQuery] int? projectId, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string status, [FromQuery] int? priority, [FromQuery] string dateSort, [FromQuery] string prioritySort)
        {
            if (page == null)
            {
                var url = new StringBuilder().Append("~/api/tasks?page=1");
                if (projectId != null)
                    url.Append("&projectid=")
                       .Append(projectId);
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

            var queryParameters = new QueryParameters(projectId, startDate, endDate, status, priority, dateSort, prioritySort);
            if ((queryParameters.DateSort != null) && (queryParameters.PrioritySort != null))
                return BadRequest("You can't sort by both parameters simultaneously");

            SortType? sortType = null;
            if (queryParameters.DateSort != null)
                sortType = SortType.Date;
            else if (queryParameters.PrioritySort != null)
                sortType = SortType.Priority;
            
            var allTasks = db.Tasks.ToList();
            var tasks = allTasks
                .Where(t =>
                {
                    var queryProjectId = queryParameters.ProjectId;
                    if (queryProjectId != null)
                        return t.ProjectId == queryProjectId;
                    else return true;
                })
                .Where(t => (t.CreationDate > queryParameters.StartDate) &&
                            (t.CreationDate < queryParameters.EndDate))
                .Where(t =>
                {
                    var queryStatus = queryParameters.Status;
                    if (queryStatus != null)
                        return t.Status == queryStatus;
                    else return true;
                })
                .Where(t =>
                {
                    var queryPriority = queryParameters.Priority;
                    if (queryPriority != null)
                        return t.Priority == queryPriority;
                    else return true;
                });
            if (sortType == SortType.Date)
            {
                if (queryParameters.DateSort == QueryParameters.Sort.Asc)
                    tasks = tasks.OrderBy(t => t.CreationDate);
                else if (queryParameters.DateSort == QueryParameters.Sort.Desc)
                    tasks = tasks.OrderByDescending(t => t.CreationDate);
            }
            else if (sortType == SortType.Priority)
            {
                if (queryParameters.PrioritySort == QueryParameters.Sort.Asc)
                    tasks = tasks.OrderBy(t => t.Priority);
                else if (queryParameters.PrioritySort == QueryParameters.Sort.Desc)
                    tasks = tasks.OrderByDescending(t => t.Priority);
            }

            var count = 10;
            var index = (int) page - 1;
            var tasksList = tasks.ToList();

            int elemCount;

            if (tasksList.Count - index * count < count)
                elemCount = tasksList.Count - index * count;
            else elemCount = count;
            
            if (elemCount <= 0)
                return NotFound();
            
            return tasksList.GetRange(index*count, elemCount);
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = (Task)db.Find(typeof(Task), id);
            if (task == null)
                return NotFound();
            else
                return task;
        }
    
        // POST api/tasks
        [HttpPost]
        public ActionResult<Task> Post([FromBody] Task postedTask)
        {
            var project = (Project) db.Find(typeof(Project), postedTask.ProjectId);
            if (project == null)
                return BadRequest("No project to attach task to");
            
            postedTask.Status = TaskStatus.New;
            db.Add(postedTask);
            db.SaveChanges();
            return postedTask;
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public ActionResult<Task> Put(int id, [FromBody] PutTask postedTask)
        {
            var task = (Task)db.Find(typeof(Task), id);
            if (task == null)
                return NotFound();
            
            if (!task.IsModifiable())
                return BadRequest("Closed tasks are not modifiable");

            if (postedTask.ProjectId != null)
            {
                var project = (Project) db.Find(typeof(Project), postedTask.ProjectId);
                if (project == null)
                    return BadRequest("No project to attach task to");
            }

            db.Update(task);
            task.UpdateWith(postedTask);
            db.SaveChanges();
            return task;
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var task = (Task)db.Find(typeof(Task), id);
            if (task == null)
                return NotFound();
            db.Remove(task);
            db.SaveChanges();
            return Ok();
        }
    }
    public class QueryParameters
        {
    
            public enum Sort
            {
                Asc,
                Desc
            }
    
            public int? ProjectId { get; private set; }
            public DateTime? StartDate { get; private set; }
            public DateTime? EndDate { get; private set; }
            public TaskStatus? Status { get; private set; }
            public int? Priority { get; private set; }
            public Sort? DateSort { get; private set; }
            public Sort? PrioritySort { get; private set; }

            public QueryParameters(int? projectId, string startDate, string endDate, string status, int? priority, string dateSort,
                string prioritySort)
            {
                ProjectId = projectId;
                
                var dateFormat = "ddMMyyyy";
                DateTime tempDate;
                
                StartDate = DateTime.TryParseExact(startDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out tempDate) ? tempDate : DateTime.MinValue;
                
                EndDate = DateTime.TryParseExact(endDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out tempDate) ? tempDate : DateTime.MaxValue;
                
                switch (status)
                {
                    case "new":
                        Status = TaskStatus.New;
                        break;
                    case "inprogress":
                        Status = TaskStatus.InProgress;
                        break;
                    case "closed":
                        Status = TaskStatus.Closed;
                        break;
                    default:
                        Status = null;
                        break;
                }

                Priority = priority;
                
                switch (dateSort)
                {
                    case "asc":
                        DateSort = Sort.Asc;
                        break;
                    case "desc":
                        DateSort = Sort.Desc;
                        break;
                    default:
                        DateSort = null;
                        break;
                }
                
                switch (prioritySort)
                {
                    case "asc":
                        PrioritySort = Sort.Asc;
                        break;
                    case "desc":
                        PrioritySort = Sort.Desc;
                        break;
                    default:
                        PrioritySort = null;
                        break;
                }
            }
        }
}