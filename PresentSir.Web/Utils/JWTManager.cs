using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using PresentSir.Web.Models;
using System;
using System.Collections.Generic;

namespace PresentSir.Web.Utils
{
    public class JWTManager
    {
        private const string secret = "1MtkQWOhYfOvgvDvdkeGzqiq9OT8RavZ";

        public enum TokenStatus
        {
            Valid,
            Invalid,
            Expired
        }

        public static string GetToken(ApplicationUser user)
        {
            var now = new UtcDateTimeProvider().GetNow();
            var payload = new Dictionary<string, object>
            {
                { "iat", Math.Round((now - JwtValidator.UnixEpoch).TotalSeconds) },
                { "exp", Math.Round((now.AddDays(1) - JwtValidator.UnixEpoch).TotalSeconds) },
                { "user_id", user.Id }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        public static string GetUserIdFromToken(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            var json = decoder.Decode(token, secret, false);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return payload["user_id"] as string;
        }

        internal static Tuple<bool, TokenStatus, int> DecodeToken(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            try
            {
                var json = decoder.Decode(token, secret, verify: true);
                var payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                return Tuple.Create(true, TokenStatus.Valid, int.Parse(payload["user_id"].ToString()));
            }
            catch (TokenExpiredException)
            {
                var json = decoder.Decode(token, secret, false);
                var payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                return Tuple.Create(false, TokenStatus.Expired, int.Parse(payload["user_id"].ToString()));
            }
            catch (SignatureVerificationException)
            {
                return Tuple.Create(false, TokenStatus.Invalid, 0);
            }
        }
    }
}