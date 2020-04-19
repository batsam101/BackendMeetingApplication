using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MeetingApp.Models;

namespace MeetingApp.DAL
{
    public class MeetingContext: DbContext
    {
        public MeetingContext():base("MeetingContext")
        { }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingItem> MeetingItems{ get; set; }
        public DbSet<MeetingType> MeetingTypes { get; set; }
        public DbSet<MeetingItemStatus> MeetingItemStatuses { get; set; }
    }
}