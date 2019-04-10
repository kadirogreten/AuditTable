# Audit Table For Asp.Net MVC
Implementing Audit Trails using ASP.NET MVC ActionFilters


The audit table shows which IP address the people have entered into the website, which user name they entered, which browser they used, which pages were clicked on the website.


# 1- Create a Audit Model

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
    
    
# 2 - Put the attribute in the homecontroller
    
    [Audit]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
    
    
    
