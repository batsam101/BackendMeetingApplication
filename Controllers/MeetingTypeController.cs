using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MeetingApp.DAL;
using MeetingApp.Models;

namespace MeetingApp.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MeetingTypeController : ApiController
    {
        [HttpPost]
        [Route("meetingtype/add")]
        public IHttpActionResult createMeetingType([FromBody]MeetingType meetingType)
        {
            if(meetingType == null)
            {
                return BadRequest("Meeting type sent to server is empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                using(var entities = new MeetingContext())
                {
                    var m = entities.MeetingTypes.FirstOrDefault(x => x.Name.ToLower() == meetingType.Name.ToLower());
                    //meetingtype does not exist
                    if (m==null)
                    {
                        entities.MeetingTypes.Add(meetingType);
                        entities.SaveChanges();
                        return Created("Unknown URL", meetingType);
                    }
                    else
                    {
                        return BadRequest("Meeting Type already exists in database");
                    }

                }
            }

            
        }

        [HttpGet]
        [Route("meetingtype/all")]
        public IHttpActionResult getMeetingTypes()
        {
            using(var entities = new MeetingContext())
            {
                var meetingtypelist = entities.MeetingTypes.ToList();
                if(meetingtypelist == null)
                {
                    return BadRequest("No meeting types exist in the database");
                }
                else
                {
                    return Ok(meetingtypelist);
                }

            }
        }
    }
}
