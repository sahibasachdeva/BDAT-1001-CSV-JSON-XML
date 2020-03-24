using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3.Model
{
    public class Constants
    {

        public readonly Student Student = new Student { StudentId = "200449112", FirstName = "Sahiba", LastName = "Sachdeva" };

        public class Locations
        {
            public readonly static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            public readonly static string ExePath = Environment.CurrentDirectory;

            public readonly static string ContentFolder = $"{ExePath}\\..\\..\\..\\Content";
            public readonly static string DataFolder = $"{ContentFolder}\\Data";
            public readonly static string ImagesFolder = $"{ContentFolder}\\Images";
            public readonly static string filePath = $@"{Constants.Locations.DataFolder}\{Constants.Locations.InfoFile}";
           

            public const string InfoFile = "info.csv";
            public const string ImageFile = "myimage.jpg";
        }

        public class FTP
        {
            public const string Username = @"bdat100119f\bdat1001";
            public const string Password = "bdat1001";

            public const string BaseUrl = "ftp://waws-prod-dm1-127.ftp.azurewebsites.windows.net/bdat1001-20914";

            public const int OperationPauseTime = 10000;
            public static string remoteUploadFileDestination = "/200449112 Sahiba Sachdeva/students.";
        }
    }
}
