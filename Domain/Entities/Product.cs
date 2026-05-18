using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Product
    {

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? Category { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }


  

    }
}
