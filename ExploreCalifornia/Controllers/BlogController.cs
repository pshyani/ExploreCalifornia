using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.Controllers
{
    //[Route("blog")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return new ContentResult { Content = "Hello" };
            //return View();
        }

        [Route("blog/{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int key, int month)
        {
            return new ContentResult { Content = string.Format("Year = {0} Month = {1} Day = {2}", year,month,key )};
            //return View();
        }
    }
}