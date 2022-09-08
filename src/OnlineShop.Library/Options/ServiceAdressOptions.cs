﻿namespace OnlineShop.Library.Options
{
    public class ServiceAdressOptions
    {
        public const string SectionName = nameof(ServiceAdressOptions);
        public string IdentityServer { get; set; }
        public string UserManagementService { get; set; }
        public string OrdersService { get; set; }
        public string ArticlesService { get; set; }
    }
}
