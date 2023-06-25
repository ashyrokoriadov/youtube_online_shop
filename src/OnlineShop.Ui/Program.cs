using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineShop.Ui;
using OnlineShop.Ui.Components;
using OnlineShop.Ui.Components.Abstractions;
using OnlineShop.Ui.States;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new("http://localhost:5008") });
builder.Services.AddSingleton<ILoginStatusManager, LoginStatusManager> ();
builder.Services.AddSingleton<IOrdersManager, OrdersManager>();

builder.Services.AddScoped<CartState>();

builder.Services.AddTransient<IArticlesProvider, ArticlesProvider>();

await builder.Build().RunAsync();
