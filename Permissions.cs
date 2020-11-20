﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace Etch.OrchardCore.Lever
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageLeverSettings = new Permission("ManageLeverSettings", "Manage Lever Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageLeverSettings }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageLeverSettings }
                }
            };
        }

    }
}
