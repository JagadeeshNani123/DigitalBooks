﻿using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
