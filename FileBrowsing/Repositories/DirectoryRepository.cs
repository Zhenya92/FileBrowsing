using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Web;
using FileBrowsing.Models;

namespace FileBrowsing.Repositories
{
    public class DirectoryRepository
    {
        public enum FileSize : long { Less10 = 1024 * 1024 * 10, Between10 = 1024 * 1024 * 10, Between50 = 1024 * 1024 * 50, MoreThan100 = 1024 * 1024 * 100 }

        public Search GetLogicalDriversModel()
        {
            Search model = new Search();
            model.CurrentPath = "...";
            model.BaseFolder = new FileFromDirectory();
            model.currendDirInfo.ParentFolder = new FileFromDirectory();
            List<FileFromDirectory> logicalDrivers = new List<FileFromDirectory>();
            foreach (string logicalDisk in Directory.GetLogicalDrives())
            {
                DirectoryInfo di = new DirectoryInfo(logicalDisk);
                logicalDrivers.Add(new FileFromDirectory() { FullName = di.FullName, Name = di.Name });
            }
            model.currendDirInfo.Directories = logicalDrivers;
            return model;
        }

        public Search CreateModel(string path)
        {
            // DirectoryInfo di = new DirectoryInfo(directory);
            //long count = di.GetFiles("",SearchOption.AllDirectories).Where(f => f.Length < fileSize).Count();   
            Search model = new Search();
            model.CurrentPath = path;
            model.currendDirInfo.Files = new List<FileFromDirectory>();
            DirectoryInfo dir = new DirectoryInfo(path);//dir = dir.parent is it a catalog?
            model.currendDirInfo.ParentFolder = new FileFromDirectory();
            model.currendDirInfo.ParentFolder.FullName = dir.Parent != null ? dir.Parent.FullName : null;
            model.currendDirInfo.ParentFolder.Name = "..";
            model.BaseFolder = new FileFromDirectory();
            model.BaseFolder.FullName = "...";
            model.BaseFolder.Name = "...";
            model.currendDirInfo.NumberOfFiles = new NumberOfFiles();
            if (File.Exists(path))
                model.currendDirInfo.CurrendDir = dir.Parent.FullName;
            else
                model.currendDirInfo.CurrendDir = dir.FullName;
            model.currendDirInfo.NumberOfFiles = new NumberOfFiles();
            CheckAllDirectories(dir.FullName, model.currendDirInfo.NumberOfFiles);

            model.currendDirInfo.Files = GetFilesFromDirectory(model.currendDirInfo.CurrendDir);
            model.currendDirInfo.Directories = GetSubdirectoriesFromDirectory(model.currendDirInfo.CurrendDir);
            return model;
        }


        public static void CheckAllDirectories(string directory, NumberOfFiles allFiles)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (dirInfo.Exists)
            {
                //check in current folder
                CheckFilesInSubDir(dirInfo, allFiles);

                //check in subfolders
                foreach (var dir in dirInfo.GetDirectories())
                {
                    /*
                    bool error = false;
                    if(dir.Attributes.HasFlag(FileAttributes.Hidden))
                        error = true;
                    
                    bool access = dir.GetAccessControl().AreAuditRulesProtected;
                    access = dir.GetAccessControl().AreAccessRulesCanonical;*/

                    try
                    {
                        CheckFilesInSubDir(dir, allFiles);
                        CheckAllDirectories(dir.FullName, allFiles);
                    }
                    catch (UnauthorizedAccessException UAEx)
                    {
                        Console.WriteLine(UAEx.Message);
                    }
                    catch (PathTooLongException PathEx)
                    {
                        Console.WriteLine(PathEx.Message);
                    }
                }
            }
            return;
        }

        public static void CheckFilesInSubDir(DirectoryInfo directory, NumberOfFiles allFiles)
        {
            if (directory.GetFiles().Any())
            {
                foreach (var file in directory.GetFiles())
                {
                    try
                    {
                        long size = file.Length <= (long)FileSize.Less10
                                ? 10 : file.Length > (long)FileSize.Between10 && file.Length <= (long)FileSize.Between50
                                ? 50 : file.Length >= (long)FileSize.MoreThan100
                                ? 100 : 0;
                        switch (size)
                        {
                            case 10: allFiles.Less10 = allFiles.Less10 + 1; break;
                            case 50: allFiles.Between10And50 = allFiles.Between10And50 + 1; break;
                            case 100: allFiles.MoreThan100 = allFiles.MoreThan100 + 1; break;
                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (UnauthorizedAccessException UAEx)
                    {
                        Console.WriteLine(UAEx.Message);
                    }
                    catch (PathTooLongException PathEx)
                    {
                        Console.WriteLine(PathEx.Message);
                    }

                }
            }
        }

        public List<FileFromDirectory> GetFilesFromDirectory(string directory)
        {
            List<FileFromDirectory> files = new List<FileFromDirectory>();
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            foreach (var file in dirInfo.GetFiles())
            {
                try
                {
                    if (file != null)
                        files.Add(new FileFromDirectory() { Name = file.Name, FullName = file.FullName });
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (UnauthorizedAccessException UAEx)
                {
                    Console.WriteLine(UAEx.Message);
                }
                catch (PathTooLongException PathEx)
                {
                    Console.WriteLine(PathEx.Message);
                }
            }
            return files;
        }

        public List<FileFromDirectory> GetSubdirectoriesFromDirectory(string directory)
        {
            List<FileFromDirectory> directories = new List<FileFromDirectory>();
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            {
                foreach (var dir in dirInfo.GetDirectories())
                    try
                    {
                        directories.Add(new FileFromDirectory() { Name = dir.Name, FullName = dir.FullName });
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (UnauthorizedAccessException UAEx)
                    {
                        Console.WriteLine(UAEx.Message);
                    }
                    catch (PathTooLongException PathEx)
                    {
                        Console.WriteLine(PathEx.Message);
                    }
            }
            return directories;
        }

        public bool Exists(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return true;
            else
                return false;
        }
    }
}