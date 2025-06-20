using System.ComponentModel.DataAnnotations;

namespace CryptoAPI.DTOs
{
    public class NoteRequest
    {
        [Required]
        public string CoinId { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Note { get; set; } = string.Empty;

        [Required]
        [RegularExpression("bullish|neutral|bearish", ErrorMessage = "Mood must be bullish, neutral, or bearish.")]
        public string Mood { get; set; } = string.Empty;
    }
}
