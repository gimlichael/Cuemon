﻿using System.Security.Claims;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Represents the method that defines an Authenticator typically assigned on <see cref="DigestAccessAuthenticationOptions"/>.
    /// </summary>
    /// <param name="username">The username to match and lookup the paired <paramref name="password"/>.</param>
    /// <param name="password">The password paired with <paramref name="username"/>.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> that is associated with the result of <paramref name="password"/>.</returns>
    public delegate ClaimsPrincipal DigestAccessAuthenticator(string username, out string password);
}