using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.DTOs
{
    public class PortfolioDto
    {
        public ICollection<PortfolioItemDto> PortfolioItems { get; set; } = new List<PortfolioItemDto>();
    }
}
