namespace SharedProject.Response
{
    public class UserManagerResponse : ApiBaseResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
    }
}
