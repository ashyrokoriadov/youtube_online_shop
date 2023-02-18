#https://learn.microsoft.com/ru-ru/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-7.3
#Set-ExecutionPolicy -ExecutionPolicy Unrestricted  -Scope CurrentUser 
$env:ASPNETCORE_ENVIRONMENT = 'Docker'; 
dotnet test 'D:\Workspace\youtube_online_shop\src\OnlineShop.OrdersService.ApiTests\OnlineShop.OrdersService.ApiTests.csproj';
Remove-Item Env:\ASPNETCORE_ENVIRONMENT;
