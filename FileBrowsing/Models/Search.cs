using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowsing.Models
{
    public class Search
    {
        public Search()
        {
            currendDirInfo = new CurrendDirectoryInfo();
        }

        public string CurrentPath { get; set; }
        public FileFromDirectory BaseFolder { get; set; }
        public CurrendDirectoryInfo currendDirInfo { get; set; }
    }

    public class FileFromDirectory
    {
        public string Name { get; set; }
        public string FullName { get; set; }
    }

    public class NumberOfFiles
    {
        public long Less10 { get; set; }
        public long Between10And50 { get; set; }
        public long MoreThan100 { get; set; }
    }

    public class CurrendDirectoryInfo
    {
        public string CurrendDir { get; set; }
        public FileFromDirectory ParentFolder { get; set; }
        public List<FileFromDirectory> AllFiles { get; set; }
        public NumberOfFiles NumberOfFiles { get; set; }
        public List<FileFromDirectory> Files { get; set; }
        public List<FileFromDirectory> Directories { get; set; }
    }
}