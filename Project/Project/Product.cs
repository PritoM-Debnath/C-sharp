using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    internal class Product
    {
        private string id;
        private string name;
        private double price;
        private int quantity;


        internal string Id
        {
            get;
            set;           
        }
        internal string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        internal double Price
        {
            get { return this.price; }
            set { this.price = value; }
        }
        internal int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }

        internal Product()
        {

        }
        internal Product(string id,string name, double price, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
        }
    }
}
