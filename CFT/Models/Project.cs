using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CFT.Models
{
    public class Project
    {
        private string _name;
        private string _description;

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

        public DateTime CreationDate { get; private set; }
        public DateTime ModificationDate { get; private set; }

        [JsonIgnore]
        public List<Task> Tasks { get; set; }

        public Project()
        {
            CreationDate = ModificationDate = DateTime.Now;
        }

        public void UpdateWith(PutProject putProject)
        {
            if (putProject.Name != null)
                putProject.Name = putProject.Name;
            if (putProject.Description != null)
                putProject.Description = putProject.Description;
        }
        
        private void Apply()
        {
            ModificationDate = DateTime.Now;
        }
    }

    public class PutProject
    {
        public string Name { set; get; }
        public string Description { set; get; }
    }
}