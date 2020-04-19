using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using MeetingApp.DAL;
using MeetingApp.Models;

namespace MeetingApp.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MeetingController : ApiController
    {
        [HttpGet]
        [Route("meeting/all")]
        public IHttpActionResult getAllMeetings()
        {
            using(var entities = new MeetingContext())
            {
                var meetinglist = entities.Meetings.ToList();
                return Ok(meetinglist);
            }
        }

        [HttpGet]
        [Route("meeting/{name}/all")]
        public IHttpActionResult getMeetingsandItemswithStatus(string name)
        {
            using (var entities = new MeetingContext())
            {
                var meeting = entities.Meetings.FirstOrDefault(x => x.Name == name);
                var meeting_data = (from m in entities.Meetings
                                    where m.Name == name
                                    join items in entities.MeetingItems on m.Id equals items.MeetingId
                                    select new {meetingname = m.Name, meetingitemdescription=items.Description }).ToList();

                if (meeting_data == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(meeting_data);
                }
            }

        }



        [HttpGet]
        [Route("meeting/currentlist/{Id}")]
        public IHttpActionResult getCurrentMeetingItemList(int Id)
        {
            using (var entities = new MeetingContext())
            {
                //get the current meeting type
                var meeting = entities.Meetings.OrderByDescending(x => x.DateofMeeting).FirstOrDefault(x => x.Id == Id);

                if (meeting == null)
                {
                    return NotFound();
                }
                else
                {

                    var meeting_data = (from m in entities.Meetings
                                        join mi in entities.MeetingItems on m.Id equals mi.MeetingId
                                        join mis in entities.MeetingItemStatuses on mi.Id equals mis.MeetingItemId
                                        where m.Name == meeting.Name
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

                                        }).OrderByDescending(x => x.meetingitemId).ThenByDescending(y => y.meetingitemstatusupdatedate).ToList();
                    var data = meeting_data.GroupBy(x => x.meetingitemId).Select(y => y.OrderByDescending(d => d.meetingitemstatusupdatedate).FirstOrDefault()).ToList();

                    return Ok(data);
                }
            }

        }


        [HttpGet]
        [Route("meeting/list/{Id}")]
        public IHttpActionResult getPrevMeetingItemListbyId(int Id)
        {
            using (var entities = new MeetingContext())
            {
                //get the last meeting of the type
                var meeting = entities.Meetings.OrderByDescending(x=>x.DateofMeeting).FirstOrDefault(x => x.MeetingTypeId == Id);

                if (meeting == null)
                {
                    return NotFound();
                }
                else
                {
                    
                    var meeting_data = (from m in entities.Meetings
                                        join mi in entities.MeetingItems on m.Id equals mi.MeetingId
                                        join mis in entities.MeetingItemStatuses on mi.Id equals mis.MeetingItemId
                                        where m.Name == meeting.Name
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
                                            
                                        }).OrderByDescending(x => x.meetingitemId).ThenByDescending(y => y.meetingitemstatusupdatedate).ToList();
                    var data = meeting_data.GroupBy(x => x.meetingitemId).Select(y => y.OrderByDescending(d=>d.meetingitemstatusupdatedate).FirstOrDefault()).ToList();
                    
                    return Ok(data);
                }
            }

        }


        [HttpPost]
        [Route("meeting/add")]
        public IHttpActionResult Post([FromBody]Meeting meeting)
        {
            
            if(meeting == null)
            {
                return BadRequest("Empty Meeting object sent");
            }
            if (ModelState.IsValid)
            {
                using (var entities = new MeetingContext())
                  {
                    var meetingtype = entities.MeetingTypes.FirstOrDefault(x => x.Id == meeting.MeetingTypeId);
                    //check if have meetings for the meetingtype
                    if(entities.Meetings.Where(x=>x.MeetingTypeId == meeting.MeetingTypeId).FirstOrDefault() == null)
                    {
                        meeting.Name = meetingtype.Name + "-1";
                    }
                    else
                    {
                        //get last meeting of that type by date
                        var lastMeetingOfType = entities.Meetings.Where(x => x.MeetingTypeId == meeting.MeetingTypeId).OrderByDescending(y => y.DateofMeeting).FirstOrDefault();
                     
                        //Get Name of new meeting eg Finance-1, Finance-2
                        var meetingtypeName = lastMeetingOfType.Name.Split('-')[0];
                        var prevMeetingNumber = Int32.Parse(lastMeetingOfType.Name.Split('-')[1]);
                        var newMeetingNumber = prevMeetingNumber + 1;
                        var newMeetingName = meetingtypeName + "-" + newMeetingNumber.ToString();
                        meeting.Name = newMeetingName;
                    }

                    meeting.DateofMeeting = DateTime.Now;
                    entities.Meetings.Add(meeting);
                    entities.SaveChanges();
                    return Created("Created", meeting);
                    /*
                    //get just saved meeting
                    var meetingCurrent = entities.Meetings.FirstOrDefault(x => x.Name == meeting.Name);
                    

                    //Previous meeting item list
                    var prevmeetingitems = entities.MeetingItems.Where(x => x.MeetingId == lastMeetingOfType.Id).ToList();
                    //Update the meetingId foreach meetingitem from the previous meeting
                    foreach(var item in prevmeetingitems)
                    {
                        item.MeetingId = meetingCurrent.Id;

                        entities.SaveChanges();
                    }
                    */

                }
                
      
  
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet]
        [Route("meeting/{name}")]
        public IHttpActionResult getMeetingbyName(string name)
        {
            using(var entities = new MeetingContext())
            {
                var meeting = entities.Meetings.FirstOrDefault(x => x.Name == name);
                if(meeting == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(meeting);
                }
            }
            
        }

        
    }
}
