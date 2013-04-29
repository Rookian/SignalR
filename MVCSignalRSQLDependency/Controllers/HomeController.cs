using System.Web.Mvc;

namespace MVCSignalRSQLDependency.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var persons = _repository.GetPersons();

            return View(persons);
        }
    }

}