using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetingApp.Models;
using MeetingApp.DAL;
using System.Web.Http.Cors;

namespace MeetingApp.Controllers
{
    [EnableCors("*","*","*")]
    public class MeetingItemController : ApiController
    {

        [Route("meetingitem/update/{Id}")]
        public IHttpActionResult Put(int Id, [FromBody] MeetingItem item)
        {
            using (var entities = new MeetingContext())
            {
                var entity = entities.MeetingItems.FirstOrDefault(x => x.Id == Id);
                if(entity == null)
                {
                    return NotFound();
                }
                else
                {
                    entity.Description = item.Description;
                    entity.DueDate = item.DueDate;
                    entity.MeetingId = item.MeetingId;
                    entity.Responsible = item.Responsible;
                    entities.SaveChanges();
                    return Ok(entity);
                }
            }
        }


        [HttpPost]
        [Route("meetingitem/add")]
        public IHttpActionResult createMeetingItem([FromBody] MeetingItem item)
        {
            if(item == null)
            {
                return BadRequest("Meeting Item is empty");
            }

            if (ModelState.IsValid)
            {
                using (var entities = new MeetingContext())
                {
                    entities.MeetingItems.Add(item);
                    entities.SaveChanges();
                    return Created("MeetingItem", item);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("meetingitem/all")]
        public IHttpActionResult getAllMeetingItems()
        {
            using(var entities = new MeetingContext())
            {
                var meetingitemlist = (from m in entities.Meetings
                                       join mi in entities.MeetingItems on m.Id equals mi.MeetingId
                                       join mis in entities.MeetingItemStatuses on mi.Id equals mis.MeetingItemId
                                       orderby mi.MeetingId descending
                                       select new
                                       {
                                           meetingtypeId = m.MeetingTypeId,
                                           meetingname = m.Name,
                                           meetingId = m.Id,
                                           meetingitemId = mi.Id,
                                           meetingitemresponsible = mi.Responsible,
                                           meetingitemdescription = mi.Description,
                                           meetingitemduedate = mi.DueDate,
                                           meetingitemstatus = mis.status,
                                           meetingitemstatusupdatedate = mis.dateOfStatusUpdate,
                                       }).ToList();
                
                if(meetingitemlist == null)
                {
                    return BadRequest("No Meetingg items exist");
                }
                else
                {
                    return Ok(meetingitemlist);
                }
            }
        }

        [HttpGet]
        [Route("meetingitem/{Id}")]
        public IHttpActionResult getMeetingItemsById(int Id)
        {
            using (var entities = new MeetingContext())
            {
                var meeting = entities.Meetings.FirstOrDefault(x => x.Id == Id);

                if(meeting == null)
                {
                    return BadRequest("No meeting items exist for meeting");
                }

                else
                {
                    var meetingitems = (from mi in entities.MeetingItems
                                        join mis in entities.MeetingItemStatuses on mi.Id equals mis.MeetingItemId into mi_mis
                                        from ms in mi_mis.DefaultIfEmpty()
                                        where mi.MeetingId == Id
                                        select new
                                        {
                                            meetingitemId = mi.Id,
                                            meetingitemresponsible = mi.Responsible,
                                            meetingitemdescription = mi.Description,
                                            meetingitemduedate = mi.DueDate,
                                            meetingitemstatus = ms==null?"":ms.status,
             
                                        });
                    return Ok(meetingitems.ToList());
                }
            }
        }

    }
}
