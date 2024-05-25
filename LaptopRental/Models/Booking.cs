﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRental.Models
{
    public class Booking
    {
        public int Id  { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; }
    }
}