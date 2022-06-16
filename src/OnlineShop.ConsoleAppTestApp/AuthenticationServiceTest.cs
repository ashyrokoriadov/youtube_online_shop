﻿using IdentityModel.Client;
using Microsoft.Extensions.Options;
using OnlineShop.Library.Authentification.Models;
using OnlineShop.Library.Authentification.Requests;
using OnlineShop.Library.Clients.AuthenticationService;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.ConsoleAppTestApp
{
    public class AuthenticationServiceTest
    {
        private readonly IdentityServerClient _identityServerClient;
        private readonly UsersClient _usersClient;
        private readonly IdentityServerApiOptions _identityServerOptions;

        public AuthenticationServiceTest(
            IdentityServerClient identityServerClient,
            UsersClient usersClient,
            IOptions<IdentityServerApiOptions> options)
        {
            _identityServerClient = identityServerClient;
            _usersClient = usersClient;
            _identityServerOptions = options.Value;
        }

        public async Task<string> Run(string[] args)
        {
            var token = await _identityServerClient.GetApiToken(_identityServerOptions);          
            _usersClient.HttpClient.SetBearerToken(token.AccessToken);

            var userName = "xyz7";

            var addResult = await _usersClient.Add(new CreateUserRequest() { User = new ApplicationUser() { UserName = userName }, Password = "Password_1" });
            Console.WriteLine($"ADD: {addResult.Succeeded}");

            Thread.Sleep(100);

            var changePasswordRequest = await _usersClient.ChangePassword(new UserPasswordChangeRequest() { UserName = userName, CurrentPassword= "Password_1", NewPassword= "Password_2" });
            Console.WriteLine($"CHANGE PASSWORD: {changePasswordRequest.Succeeded}");

            Thread.Sleep(100);

            var getOneRequest = await _usersClient.Get(userName);
            Console.WriteLine($"GET ONE: {getOneRequest.Code}");

            Thread.Sleep(100);

            var userToUpdate = getOneRequest.Payload;
            userToUpdate.DefaultAddress = new Address()
            {
                City = "Warsaw",
                Country = "Poland",
                PostalCode = "00-001",
                AddressLine1 = "Jasna 21",
                AddressLine2 = "34"
            };
            var updateResult = await _usersClient.Update(userToUpdate);
            Console.WriteLine($"UPDATE: {updateResult.Succeeded}");

            Thread.Sleep(100);

            getOneRequest = await _usersClient.Get(userName);
            Console.WriteLine($"GET ONE: {getOneRequest.Code}");

            Thread.Sleep(100);

            var deleteResult = await _usersClient.Remove(getOneRequest.Payload);
            Console.WriteLine($"DELETE: {deleteResult.Succeeded}");

            Thread.Sleep(100);

            var getAllRequest = await _usersClient.GetAll();
            Console.WriteLine($"GET ALL: {getOneRequest.Code}");

            Thread.Sleep(100);

            return "OK";
        }
    }
}
