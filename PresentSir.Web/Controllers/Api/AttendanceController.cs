using PagedList;
using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PresentSir.Web.Extensions;

namespace PresentSir.Web.Controllers.Api
{
    public class AttendanceController : ApiController
    {
        public HttpResponseMessage Get(int pageNumber, int perPage, int classId, string date)
        {
            var dateArray = date.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var dateTime = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));

            var attendance = ApplicationDbContext.Instance.Attendance.Include(x => x.Student).Include(x => x.Student.User).Find(x => x.ClassId == classId && x.ClassDate.IsSameDay(dateTime)).ToPagedList(pageNumber, perPage);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Data = attendance,
                Pagination = new Pagination
                {
                    HasNextPage = attendance.HasNextPage,
                    HasPreviousPage = attendance.HasPreviousPage,
                    PageNumber = attendance.PageNumber
                }
            });
        }
    }
}