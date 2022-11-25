namespace BlockChain.Domain.Utils;

public interface IDataSigner
{
    public Task<(byte[], string)> SignData(byte[] bytesForSigning);
    public Task<bool> VerifyData(byte[] originalBytes, byte[] signedBytes);
}