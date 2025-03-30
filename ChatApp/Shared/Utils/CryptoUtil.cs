using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Shared.Exceptions;

namespace ChatApp.Shared.Utils;

public static class CryptoUtil
{
    public static byte[] ComputeSHA512Hash(string input, int inputSizeLimit)
    {
        if (inputSizeLimit < 0)
        {
            throw new Exception("Input size limit for SHA512 is negative");
        }

        if (input.Length > inputSizeLimit)
        {
            throw new ApiException("Input size for SHA512 is greater than size limit", HttpStatusCode.BadRequest);
        }

        using SHA512 sha512 = SHA512.Create();

        byte[] hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));

        return hash;

    }
}
