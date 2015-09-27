using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FileBrowsing.Models;
using FileBrowsing.Repositories;

namespace FileBrowsing.Controllers
{
    public class ValuesController : ApiController
    {
        public DirectoryRepository dirRepo = new DirectoryRepository();

        public Search GetDirectoryData(string path)
        {
            Search model = null;
            if (!string.IsNullOrEmpty(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (path == "...") return dirRepo.GetLogicalDriversModel();
                if (File.Exists(path) || Directory.Exists(path))
                {
                    model = dirRepo.CreateModel(path);
                }
            }
            return model;
        }
    }
}
