using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace Adverthouse.Core.Security;

public static class SecureURL
{
    private const int IvSize = 16;
    private const int BlockSize = 16;

    private static byte[] DeriveKey(string secretKey)
    {
        if (string.IsNullOrEmpty(secretKey))
            throw new ArgumentException(
                "Secret key cannot be empty.",
                nameof(secretKey));

        return SHA256.HashData(
            Encoding.UTF8.GetBytes(secretKey));
    }

    public static string Encrypt<T>(T payload, string secretKey)
    {
        string json = JsonConvert.SerializeObject(payload);
        byte[] rawBytes = Encoding.UTF8.GetBytes(json);

        // GZip
        byte[] compressedBytes;

        using (var ms = new MemoryStream())
        {
            using (var gzip = new GZipStream(
                ms,
                CompressionLevel.Optimal,
                leaveOpen: true))
            {
                gzip.Write(rawBytes, 0, rawBytes.Length);
            }

            compressedBytes = ms.ToArray();
        }

        // AES
        using var aes = Aes.Create();

        aes.Key = DeriveKey(secretKey);
        aes.GenerateIV();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptedMs = new MemoryStream();

        // IV
        encryptedMs.Write(aes.IV, 0, aes.IV.Length);

        using (var cryptoStream = new CryptoStream(
            encryptedMs,
            aes.CreateEncryptor(),
            CryptoStreamMode.Write,
            leaveOpen: true))
        {
            cryptoStream.Write(
                compressedBytes,
                0,
                compressedBytes.Length);

            cryptoStream.FlushFinalBlock();
        }

        byte[] encryptedBytes = encryptedMs.ToArray();

        // RAW BYTES -> Base64URL
        return Base64UrlEncoder.Encode(encryptedBytes);
    }
    public static T Decrypt<T>(
        string base64UrlPayload,
        string secretKey)
    {
        if (string.IsNullOrWhiteSpace(base64UrlPayload))
            throw new ArgumentException(
                "Payload cannot be empty.",
                nameof(base64UrlPayload));

        // Base64URL -> RAW BYTES
        byte[] encryptedBytes =
            Base64UrlEncoder.DecodeBytes(base64UrlPayload);

        const int ivSize = 16;

        if (encryptedBytes.Length <= ivSize)
            throw new CryptographicException(
                "Encrypted payload is too short.");

        int cipherLength =
            encryptedBytes.Length - ivSize;

        if (cipherLength % 16 != 0)
            throw new CryptographicException(
                $"Invalid AES-CBC ciphertext length: {cipherLength} bytes.");

        // IV
        byte[] iv = new byte[ivSize];

        Buffer.BlockCopy(
            encryptedBytes,
            0,
            iv,
            0,
            ivSize);

        // Ciphertext
        byte[] ciphertext = new byte[cipherLength];

        Buffer.BlockCopy(
            encryptedBytes,
            ivSize,
            ciphertext,
            0,
            cipherLength);

        // AES decrypt
        byte[] compressedBytes;

        using (var aes = Aes.Create())
        {
            aes.Key = DeriveKey(secretKey);
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var input =
                new MemoryStream(ciphertext);

            using var cryptoStream =
                new CryptoStream(
                    input,
                    aes.CreateDecryptor(),
                    CryptoStreamMode.Read);

            using var output =
                new MemoryStream();

            cryptoStream.CopyTo(output);

            compressedBytes = output.ToArray();
        }

        // GZip
        byte[] rawBytes;

        using (var input =
            new MemoryStream(compressedBytes))
        using (var gzip =
            new GZipStream(
                input,
                CompressionMode.Decompress))
        using (var output =
            new MemoryStream())
        {
            gzip.CopyTo(output);
            rawBytes = output.ToArray();
        }

        // JSON
        string json =
            Encoding.UTF8.GetString(rawBytes);

        return JsonConvert.DeserializeObject<T>(json)!;
    }
}