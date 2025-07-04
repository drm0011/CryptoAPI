﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DTOs
{
    public class PortfolioItemDto
    {
        [Required(ErrorMessage = "CoinId is required.")]
        public string CoinId { get; set; }

        [Required(ErrorMessage = "CoinName is required.")]
        public string CoinName { get; set; }
    }
}
