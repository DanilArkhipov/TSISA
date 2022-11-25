

using System.Security.Cryptography;
using System.Text.Json;

namespace BlockChain.Domain.Utils;

public class DataArbiterSigner: IDataSigner
{
    public async Task<(byte[], string)> SignData(byte[] bytesForSigning)
    {
        var httpClient = new HttpClient();
        var hexString = BitConverter.ToString(bytesForSigning).Replace("-","");
        var requestUrl = new Uri($"http://89.108.115.118/ts?digest={hexString}");
        var response = await httpClient.GetAsync(requestUrl);
        var responseString = await response.Content.ReadAsStringAsync();
        var answerData =  JsonSerializer.Deserialize<ArbiterAnswer>(responseString);

        if (answerData.Status != 0)
        {
            throw new Exception(answerData.StatusString);
        }
        
        var signatureBytes = Enumerable.Range(0, answerData.TimeStampToken.Signature.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(answerData.TimeStampToken.Signature.Substring(x, 2), 16))
            .ToArray();
        return (signatureBytes, answerData.TimeStampToken.Ts);
    }

    public async Task<bool> VerifyData(byte[] originalBytes, byte[] signedBytes)
    {
        var publicKey = await GetPublicRsaKey();
        var byteArray = Convert.FromBase64String(publicKey);

        using var rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(byteArray, out _);

        var rsaParameters = rsa.ExportParameters(false);
        bool success = false;
        using (var rsaProvider = new RSACryptoServiceProvider())
        {
            try
            {
                rsaProvider.ImportParameters(rsaParameters);

                success = rsaProvider.VerifyData(originalBytes, CryptoConfig.MapNameToOID("SHA256"), signedBytes);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                rsaProvider.PersistKeyInCsp = false;
            }
        }
        return success;
    }

    private async Task<string> GetPublicRsaKey()
    {
        var httpClient = new HttpClient();
        var requestUrl = new Uri("http://89.108.115.118/ts/public64");
        var response = await httpClient.GetAsync(requestUrl);
        return await response.Content.ReadAsStringAsync();
    }
}