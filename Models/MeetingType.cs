using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingApp.Models
{
    public class MeetingType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}