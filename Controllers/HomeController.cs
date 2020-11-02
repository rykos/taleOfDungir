using Microsoft.AspNetCore.Mvc;

namespace taleOfDungir.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public string Index()
        {
            return "";
        }
    }
}