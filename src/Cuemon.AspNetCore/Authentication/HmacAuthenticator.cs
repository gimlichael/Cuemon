using System.Security.Claims;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Represents the method that defines an Authenticator typically assigned on <see cref="HmacAuthenticationOptions"/>.
    /// </summary>
    /// <param name="publicKey">The public key to match and lookup the paired shared secret-<paramref name="privateKey"/>.</param>
    /// <param name="privateKey">The shared secret-private key paired with <paramref name="publicKey"/>.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> that is associated with the result of <paramref name="privateKey"/>.</returns>
    public delegate ClaimsPrincipal HmacAuthenticator(string publicKey, out byte[] privateKey);
}