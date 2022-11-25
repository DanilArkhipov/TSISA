using System.Text;
using BlockChain.Domain.Utils;

namespace BlockChain.Data.Entity;

public class Block
{
    private readonly DataHasher _dataHasher;
    private readonly IDataSigner _dataSigner;
    
    public int BlockId { get; private set; }

    public string Data { get; private set; }
    
    public byte[] Hash { get; private set; }
    
    public byte[] DataSign { get; private set; }
    
    public string DataSignTimeStamp { get; private set; }
    
    public byte[] HashSign { get; private set; }
    
    public string HashSignTimeStamp { get; private set; }

    private Block()
    {
        _dataSigner = new DataSigner();
        _dataHasher = new DataHasher();
    }

    public Block(string data) : this()
    {
        var dataSignResult = _dataSigner.SignData(Encoding.UTF8.GetBytes(data)).Result;
        var hash = _dataHasher.GetHash(data + Convert.ToBase64String(dataSignResult.Item1));
        var hashSignResult = _dataSigner.SignData(hash).Result;
        Data = data;
        DataSign = dataSignResult.Item1;
        DataSignTimeStamp = dataSignResult.Item2;
        Hash = hash;
        HashSign = hashSignResult.Item1;
        HashSignTimeStamp = hashSignResult.Item2;
    }

    public Block(string data, Block prevBlock): this()
    {
        var dataSignResult = _dataSigner.SignData(Encoding.UTF8.GetBytes(data)).Result;
        var hash = _dataHasher.GetHash(data + Convert.ToBase64String(dataSignResult.Item1) + Convert.ToBase64String(prevBlock.Hash));
        var hashSignResult = _dataSigner.SignData(hash).Result;
        Data = data;
        DataSign = dataSignResult.Item1;
        DataSignTimeStamp = dataSignResult.Item2;
        Hash = hash;
        HashSign = hashSignResult.Item1;
        HashSignTimeStamp = hashSignResult.Item2;
    }

    public async Task<bool> Verify(Block prevBlock = null)
    {
        var dataBytes = Encoding.UTF8.GetBytes(Data);
        if (!await  _dataSigner.VerifyData(dataBytes, DataSign))
        {
            return false;
        }

        var prevBlockHashString = prevBlock is not null ? Convert.ToBase64String(prevBlock.Hash) : "";
        var dataHashBytes = _dataHasher.GetHash(Data + Convert.ToBase64String(DataSign) + prevBlockHashString);
        
        if (!await  _dataSigner.VerifyData(dataHashBytes, HashSign))
        {
            return false;
        }

        return true;
    }
}