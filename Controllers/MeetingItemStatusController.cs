using MeetingApp.DAL;
using MeetingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MeetingApp.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MeetingItemStatusController : ApiController
    {
        [HttpPost]
        [Route("meetingitemstatus/add")]
        public IHttpActionResult Post([FromBody]MeetingItemStatus status)
        {
            if (status == null)
            {
                return BadRequest("Empty status object sent");
            }
            if (ModelState.IsValid)
            {
                using (var entities = new MeetingContext())
                {
                    status.dateOfStatusUpdate = DateTime.Now;
                    entities.MeetingItemStatuses.Add(status);
                    try
                    {
                        entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                    return Created("Created", status);
                }



            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}
