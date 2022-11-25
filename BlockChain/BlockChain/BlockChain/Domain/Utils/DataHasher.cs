using System.Text;
using System.Security.Cryptography;

namespace BlockChain.Domain.Utils;

public class DataHasher
{
    public byte[] GetHash(string data)
    {
        using (SHA256 sha256Hash = SHA256.Create())  
        {  
            return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));  
        }
    }
}