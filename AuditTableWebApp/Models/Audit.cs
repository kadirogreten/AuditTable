using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuditTableWebApp.Models
{
    public class Audit
    {
        public Guid AuditID { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime Timestamp { get; set; }
        public string Browser { get; set; }

        // Default Constructor
        public Audit() { }



    }

    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;
            // Generate an audit
            Audit audit = new Audit()
            {
                // Your Audit Identifier     
                AuditID = Guid.NewGuid(),

                // Our Username (if available)
                UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous",

                // The IP Address of the Request
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,

                Browser = request.Browser.Browser,

                // The URL that was accessed
                AreaAccessed = request.RawUrl,

                // Creates our Timestamp
                Timestamp = DateTime.Now
            };

            // Stores the Audit in the Database
            ApplicationDbContext context = new ApplicationDbContext();
            context.AuditRecords.Add(audit);
            context.SaveChanges();

            // Finishes executing the Action as normal 
            base.OnActionExecuting(filterContext);
        }
    }
}