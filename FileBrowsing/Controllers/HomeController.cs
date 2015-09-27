using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using FileBrowsing.Models;
using FileBrowsing.Repositories;

namespace FileBrowsing.Controllers
{
    public class HomeController : Controller
    {
        public DirectoryRepository dirRepo = new DirectoryRepository();

        public ActionResult Index()
        {
            Search model = dirRepo.GetLogicalDriversModel();
            return View(model);
        }
    }
}
