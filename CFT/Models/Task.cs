using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CFT.Models
{
    public enum TaskStatus
    {
        New,
        InProgress,
        Closed
    }
    public class Task
    {
        private string _name;
        private string _description;
        private byte? _priority;
        private TaskStatus? _status;

        public int Id { get; private set; }

        [Required]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Apply();
            }
        }

        [Required]
        public string Description
        {
            get => _description;
            set
            {
                _description = value; 
                Apply();
            }
        }

        [Required]
        public byte? Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                Apply();
            } 
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [Required]
        public TaskStatus? Status
        {
            get => _status;
            set
            {
                _status = value;
                Apply();
            }
        }

        [Required]
        public int? ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }

        public DateTime CreationDate { get; private set; }
        public DateTime ModificationDate { get; private set; }

        public Task()
        {
            Status = TaskStatus.New;
            CreationDate = ModificationDate = DateTime.Now;
        }

        public void UpdateWith(PutTask putTask)
        {
            if (this.Status == TaskStatus.Closed)
                return;
            if (putTask.ProjectId != null)
                this.ProjectId = putTask.ProjectId;
            if (putTask.Name != null)
                this.Name = putTask.Name;
            if (putTask.Description != null)
                this.Description = putTask.Description;
            if (putTask.Priority != null)
                this.Priority = putTask.Priority;
            if (putTask.Status != null)
                this.Status = putTask.Status;
        }

        public bool IsModifiable()
        {
            return this.Status != TaskStatus.Closed;
        }

        private void Apply()
        {
            ModificationDate = DateTime.Now;
        }
    }

    public class PutTask
    {
        public int? ProjectId { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskStatus? Status { get; set; }
        public byte? Priority { get; set; }
    }
}