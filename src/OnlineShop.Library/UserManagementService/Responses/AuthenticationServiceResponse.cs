namespace OnlineShop.Library.Authentification.Responses
{
    public class AuthenticationServiceResponse<T>
    {
        public T Payload { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
