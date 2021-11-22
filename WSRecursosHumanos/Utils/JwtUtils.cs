using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using System;
using System.Collections.Generic;

namespace WSRecursosHumanos.Utils
{
    public class JwtUtils
    {
        public static String getTokenFromAuthorizationHeader(String header) {
            try {
                return header.Split()[1];
            } catch (Exception ex)
            {
                return null;
            }
        }

        public static IDictionary<string, object> decodeToken(string token)
        {
            try
            {
                var payload = JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret("themostsecurepassword")
                        .MustVerifySignature()
                        .Decode<IDictionary<string, object>>(token);
                return payload;
            }
            catch (TokenExpiredException ex)
            {
                return null;
            }
            catch (SignatureVerificationException ex) {
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}