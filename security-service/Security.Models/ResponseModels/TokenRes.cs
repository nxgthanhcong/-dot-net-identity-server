namespace Security.Models.ResponseModels
{
    public class TokenRes
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
