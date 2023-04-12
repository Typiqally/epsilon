using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = IdentityModel.Jwk.JsonWebKey;

namespace NetCore.Lti.Extensions;

public static class JsonWebKeyExtensions
{
    public static RsaSecurityKey ToRsaSecurityKey(this JsonWebKey jwk)
    {
        var rsaParameters = new RSAParameters
        {
            Modulus = TryDecodeBytes(jwk.N),
            Exponent = TryDecodeBytes(jwk.E),
            D = TryDecodeBytes(jwk.D),
            DP = TryDecodeBytes(jwk.DP),
            DQ = TryDecodeBytes(jwk.DQ),
            P = TryDecodeBytes(jwk.P),
            Q = TryDecodeBytes(jwk.Q),
            InverseQ = TryDecodeBytes(jwk.QI),
        };

        return new RsaSecurityKey(rsaParameters);
    }

    private static byte[]? TryDecodeBytes(string? str)
    {
        return str == null ? null : Base64UrlEncoder.DecodeBytes(str);
    }
}