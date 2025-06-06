﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DAL.Entities
{
    public class Portfolio
    {
        public int Id { get; init; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<PortfolioItem> PortfolioItems { get; set; } = new List<PortfolioItem>();
    }
}
