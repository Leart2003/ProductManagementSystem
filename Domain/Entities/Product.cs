using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    internal class Product
    {

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int Description { get; set; }

        public decimal Price { get; set; }

        public string? Category { get; set; }


        //        •	- ID(GUID)
        //•	- Name(string)
        //•	- Description(string)
        //•	- Price(decimal)
        //•	- Category(string)

    }
}
