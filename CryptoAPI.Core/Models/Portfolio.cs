﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class Portfolio 
    {
        public ICollection<PortfolioItem> PortfolioItems { get; set; } = new List<PortfolioItem>();
    }
}
