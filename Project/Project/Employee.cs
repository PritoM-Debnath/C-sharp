using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    internal class Employee
    {
        private string id;
        private string name;
        private DateTime joiningdate;
        private double salary;
        private string password;

        private string Password
        {
            get;
            set;
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        
        public DateTime Joiningdate
        {
            get { return this.joiningdate; }
            set { this.joiningdate = value; }
        }

        public double Salary
        {
            get { return this.salary; }
            set { this.salary = value; }
        }

        public Employee(string id,string name, string password, DateTime joiningdate, double salary)
        {
            this.Id=id;
            this.password=password;
            this.Name=name;
            this.Joiningdate=joiningdate;
            this.Salary=salary;
        }

    }
}
