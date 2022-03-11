using EMF_Portal_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Extentions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthenticationController.ConfirmEmail),
                controller: "Authentication",
                values: new { userId, code },
                protocol: scheme);
        }

        //Method not yet defind
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthenticationController.ResetPassword),
                controller: "Authentication",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
