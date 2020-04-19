using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MeetingApp.Models
{
    public class MeetingItemStatus
    {
        public int Id { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public int MeetingItemId { get; set; }

        public DateTime dateOfStatusUpdate { get; set; }

    }
}