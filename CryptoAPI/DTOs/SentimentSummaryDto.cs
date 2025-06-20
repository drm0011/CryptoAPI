using System.ComponentModel.DataAnnotations;

namespace CryptoAPI.DTOs
{
    public class SentimentSummaryDto
    {
        [Required]
        public int Bullish { get; set; }
        [Required]
        public int Neutral { get; set; }
        [Required]
        public int Bearish { get; set; }
    }
}
