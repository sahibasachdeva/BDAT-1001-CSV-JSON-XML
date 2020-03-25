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
using System.Xml.Serialization;

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
                    var imageBytes = Helper.FTP.DownloadFileBytes(student.MyImagePath);
                    Image myimage = Helper.ImageConversion.ByteArrayToImage(imageBytes);

                    string base64 = ImageConversion.ImageToBase64(myimage, ImageFormat.Jpeg);
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

            //string studentXMLPath = $"{Constants.Locations.DataFolder}\\students.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(students.GetType());


            //Serialize XML
            //Create an instacne of StringWriter to write the XML string as a Stream
            using (StringWriter textWriter = new StringWriter())
            {
                //Convert the student object to XML
                xmlSerializer.Serialize(textWriter, students);

                //Write out the XML to the Console
                string studentXMLPath = textWriter.ToString();
                Helper.FTP.UploadFile(studentXMLPath, Constants.FTP.BaseUrl + Constants.FTP.remoteUploadFileDestination + "xml", Constants.FTP.Username, Constants.FTP.Password);

            }

            string studentJSONPath = Newtonsoft.Json.JsonConvert.SerializeObject(students);


            Helper.FTP.UploadFile(studentCSVPath, Constants.FTP.BaseUrl + Constants.FTP.remoteUploadFileDestination + "csv", Constants.FTP.Username, Constants.FTP.Password);
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

            var averageAge = students.Average(x => x.age);
            var maxAge = students.Max(x => x.age);
            var minAge = students.Min(x => x.age);
            Console.WriteLine("the Average Age is " + averageAge);
            Console.WriteLine("{0} is the MAX age and {1} is the MIN age", maxAge, minAge); 
            #endregion
        }
    }
}
