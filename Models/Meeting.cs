using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MeetingApp.Models
{
    public class Meeting
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        [Required]
        public int MeetingTypeId { get; set; }
     
        public DateTime DateofMeeting { get; set; }


    }
}