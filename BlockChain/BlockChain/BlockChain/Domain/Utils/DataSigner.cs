using System.Security.Cryptography;

namespace BlockChain.Domain.Utils;

public class DataSigner: IDataSigner
{


    private Task<string> GetPrivateRsaKey()
    {
        return File.ReadAllTextAsync("SignKeys/RsaPrivateKey.txt");
    }
    
    private Task<string> GetPublicRsaKey()
    {
        return File.ReadAllTextAsync("SignKeys/RsaPublicKey.txt");
    }

    public async Task<(byte[], string)> SignData(byte[] bytesForSigning)
    {
        var key = await GetPrivateRsaKey();
        using (var rsa = new RSACryptoServiceProvider())
        {
            try
            {
                rsa.ImportFromPem(key);
                return (rsa.SignData(bytesForSigning, CryptoConfig.MapNameToOID("SHA256")), DateTime.Now.ToString());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return (null, null);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }

    public async Task<bool> VerifyData(byte[] originalBytes, byte[] signedBytes)
    {
        bool success = false;
        var publicKey = await GetPublicRsaKey();
        using (var rsa = new RSACryptoServiceProvider())
        {
            try
            {
                rsa.ImportFromPem(publicKey);

                success = rsa.VerifyData(originalBytes, CryptoConfig.MapNameToOID("SHA256"), signedBytes);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
        return success;
    }
}