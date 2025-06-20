namespace CryptoAPI.DTOs
{
    public class NoteRequest
    {
        public string CoinId { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Mood { get; set; } = string.Empty;
    }
}
