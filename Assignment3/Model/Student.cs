using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3.Model
{
    public class Student
    {
        public static string HeaderRow = $"{nameof(Student.StudentId)}, {nameof(Student.FirstName)}, {nameof(Student.LastName)}, {nameof(Student.DateOfBirth)}, {nameof(Student.ImageData)}, {nameof(Student.Age)}";
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string _DateOfBirth;
        public string DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {                         
                _DateOfBirth = value;

                //Convert DateOfBirth to DateTime
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DateOfBirthDT = dtOut;
            }
        }

        public DateTime DateOfBirthDT { get; internal set; }
        public string ImageData { get; set; }

        public string AbsoluteUrl { get; set; }
        public string Directory { get; set; }
        public string FullPathUrl
        {
            get
            {
                return AbsoluteUrl + "/" + Directory;
            }
        }


        public int age
        {
            get
            {
                int age = 0;
                if (DateOfBirthDT.Year <= 1  )
                {
                    return age = 20;
                }
                else
                {
                    age = DateTime.Now.Year - DateOfBirthDT.Year;
                    if (DateTime.Now.DayOfYear < DateOfBirthDT.DayOfYear)
                        age = age - 1;

                    return age;
                }
            }
        }

        public int Age{get; set;}
        public bool MyRecord { get; set; }
        public void FromCSV(string csvdata)
        {
            string[] data = csvdata.Split(",", StringSplitOptions.None);
            try
            {
                StudentId = data[0];
                FirstName = data[1];
                LastName = data[2];
                DateOfBirth = data[3];
                ImageData = data[4];
                //Age = CalculateAge(DateOfBirthDT);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void FromDirectory(string directory)
        {
            Directory = directory;

            if (String.IsNullOrEmpty(directory.Trim()))
            {
                return;
            }

            string[] data = directory.Trim().Split(" ", StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
        }



        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DateOfBirthDT.ToShortDateString()},{ImageData}, {age}";
            return result;
        }

        public override string ToString()
        {
            string result = $"{StudentId} {FirstName} {LastName}";
            return result;
        }

        public string ToXML()
        {
            string result = $"<StudentId> {StudentId} </StudentId> <FirstName> {FirstName} </FirstName> <LastName> {LastName} </LastName> <DOB> {DateOfBirthDT.ToShortDateString()} </DOB> <Age> {age} </Age>";
                  return result;
        }

        public string ToJSON()
        {
            string result = $"[ 'StudentId' : {StudentId} , 'FirstName' : {FirstName} ,  'LastName' : {LastName}]";
                return result;
        }
        //static int CalculateAge(DateTime dateOfBirth)
        //{
        //    int age = 0;
        //    age = DateTime.Now.Year - dateOfBirth.Year;
        //    if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
        //        age = age - 1;

        //    return age;
        //}
    }
}
