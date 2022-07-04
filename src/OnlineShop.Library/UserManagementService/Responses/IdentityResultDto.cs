using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OnlineShop.Library.UserManagementService.Responses
{
    public class IdentityResultDto
    {
        public bool Succeeded { get; set; }

        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
