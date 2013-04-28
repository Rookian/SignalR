using System.Web.Mvc;

namespace MVCSignalRSQLDependency.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repository = new Repository();
            var enumerable = repository.Get();
             
            //if (Request.IsAjaxRequest())
            //    return Json(enumerable, JsonRequestBehavior.AllowGet);
            
            return View(enumerable);
        }

       
    }

    public class Person
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public long ID { get; set; }
    }
}