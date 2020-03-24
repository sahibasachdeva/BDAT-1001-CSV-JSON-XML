using Assignment3.Model;
using System;
using System.Collections.Generic;
using Assignment3.Helper;
using System.IO;
using static Assignment3.Model.Constants;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>();
            List<string> directories = Helper.FTP.GetDirectory(Constants.FTP.BaseUrl);

            foreach (var directory in directories)
            {
                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);

                Console.WriteLine(student);
                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InfoFile;

                bool fileExists = Helper.FTP.FileExists(infoFilePath);
                if (fileExists == true)
                {
                    string csvPath = $@"C:\Users\chirag\Desktop\Students\{directory}.csv";
                    //Helper.FTP.DownloadFile(infoFilePath, csvPath);

                    var downloadedBytes = Helper.FTP.DownloadFileBytes(infoFilePath);
                    string csvData = Encoding.Default.GetString(downloadedBytes);
                    string[] csvLines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    if (csvLines.Length != 2)
                    {
                        Console.WriteLine("error in parsing");
                    }
                    else
                    {
                        student.FromCSV(csvLines[1]);
                        
                       
                    }
                    Console.WriteLine("Found info file:");
                }
                else
                {
                    Console.WriteLine("Could not find info file:");
                }

                Console.WriteLine("\t" + infoFilePath);

                string imageFilePath = student.FullPathUrl + "/" + Constants.Locations.ImageFile;

                bool imageFileExists = Helper.FTP.FileExists(imageFilePath);

                if (imageFileExists == true)
                {
                    Console.WriteLine("Found image file:");
                }
                else
                {
                    Console.WriteLine("Could not find image file:");
                }
                students.Add(student);
                Console.WriteLine("\t" + imageFilePath);
            }


            string studentCSVPath = $"{Constants.Locations.DataFolder}\\students.csv";
            using (StreamWriter fs = new StreamWriter(studentCSVPath))
            {
                foreach (var student in students)
                { 
                    fs.WriteLine(student.ToCSV());
                }
            }

            string studentXMLPath = $"{Constants.Locations.DataFolder}\\students.xml";
            using (StreamWriter fs = new StreamWriter(studentXMLPath))
            {
                foreach (var student in students)
                {
                    fs.WriteLine(student.ToXML());
                }
            }

            string studentJSONPath = $"{Constants.Locations.DataFolder}\\students.json";
            using (StreamWriter fs = new StreamWriter(studentJSONPath))
            {
                foreach (var student in students)
                {
                    fs.WriteLine(student.ToJSON());
                }
            }

            Helper.FTP.UploadFile(studentCSVPath, Constants.FTP.BaseUrl + Constants.FTP.remoteUploadFileDestination + "csv", Constants.FTP.Username, Constants.FTP.Password);
            Helper.FTP.UploadFile(studentXMLPath, Constants.FTP.BaseUrl + Constants.FTP.remoteUploadFileDestination + "xml", Constants.FTP.Username, Constants.FTP.Password);
            Helper.FTP.UploadFile(studentJSONPath, Constants.FTP.BaseUrl + Constants.FTP.remoteUploadFileDestination + "json", Constants.FTP.Username, Constants.FTP.Password);

            #region Aggregate 
            var count = students.Count();
            Console.WriteLine("count is " + count);
            var d = 0; var c= 0;
            foreach (var student in students) 
            {
                if (student.ToString().StartsWith("2004"))
                {
                   c++;
                }

                if (student.ToString().Contains("a"))
                {
                     d++;
                }  
                
                if(student.FirstName == "Sahiba" & student.StudentId == "200449112")
                {
                    student.MyRecord = true;
                }


            }
            Console.WriteLine("Count of starts With" + c);
            Console.WriteLine("Count of Contains is " + d);

            List<int> age = new List<int>();
            foreach(var stu in students)
            {
                age.Add(stu.age);
            }
            var averageAge = age.Average();
            var maxAge = age.Max();
            var minAge = age.Min();
            Console.WriteLine("the Average Age is " + averageAge);
            Console.WriteLine("{0} is the MAX age and {1} is the MIN age", maxAge, minAge); 
            #endregion
        }
    }
}
