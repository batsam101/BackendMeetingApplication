using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MeetingApp.Models
{
    public class MeetingItem
    {
        public int Id { get; set; }

        [Required]
        public int MeetingId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Responsible { get; set; }
        [Required]
        public DateTime DueDate { get; set; }

    }
}