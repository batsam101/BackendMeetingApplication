using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MeetingApp.Models;

namespace MeetingApp.DAL
{
    public class MeetingInitializer : DropCreateDatabaseIfModelChanges<MeetingContext>
    {
        protected override void Seed(MeetingContext context)
        {
           
        }
    }
}