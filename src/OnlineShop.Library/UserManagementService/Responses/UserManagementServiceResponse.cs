namespace OnlineShop.Library.Authentification.Responses
{
    public class UserManagementServiceResponse<T>
    {
        public T Payload { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
