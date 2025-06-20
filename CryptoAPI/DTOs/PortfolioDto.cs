using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DTOs
{
    public class PortfolioDto
    {
        [Required]
        public ICollection<PortfolioItemDto> PortfolioItems { get; set; } = new List<PortfolioItemDto>();
    }
}
