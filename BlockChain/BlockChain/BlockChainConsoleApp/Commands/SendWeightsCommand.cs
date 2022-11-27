using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BlockChain.Domain;
using BlockChain.Domain.Utils;
using BlockChainConsoleApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlockChainConsoleApp.Commands;

public class SendWeightsCommand
{
    private IDataSigner _dataSigner;

    private string _publicKey =
        "30819e300d06092a864886f70d010101050003818c003081880281807bdf5c18e74bea92f885010e20995491419f6f2f02a73b7ae585a0646d317e0bddcdc452306532c58df9afabd6a9ed0031b8382061c0c871d44246c46e4df6b91d565b8688a6127b02c92d84342afd3c9aa45b0f21a17caf1e684b8cef0743e23c388fc98e93decded8152976d67d4eab3d4f174ca1e993c1631dfe0982d76d90203010001";


    public SendWeightsCommand()
    {
        _dataSigner = new DataSigner();
    }

    public async Task Execute()
    {
        var weightData = new NeuralWeightsModel()
        {
            w11 = "-0.2292",
            w12 = "0.1342",
            w21 = "0.5773",
            w22 = "-0.1148",
            v11 = "-0.2025",
            v12 = "-0.1451",
            v13 = "-0.5844",
            v21 = "0.1117",
            v22 = "-0.4720",
            v23 = "-0.2808",
            w1 = "0.3002",
            w2 = "0.5394",
            w3 = "0.0641",
            e = "0.06462576122337255",
            publicKey =  _publicKey
        };
        var weightDataString = JsonSerializer.Serialize(weightData);
        var weightDataStringBytes = Encoding.UTF8.GetBytes(weightDataString);
        var sign = await _dataSigner.SignData(weightDataStringBytes);

        var blocks = await GetBlocks();
        var prevBlock = blocks[^1];
        var prevHashBytes = StringToByteArray(prevBlock.prevhash);
        var prevData = JsonSerializer.Serialize(prevBlock.data);
        var prevDataBytes = Encoding.UTF8.GetBytes(prevData);
        var prevDataSignBytes = StringToByteArray(prevBlock.signature);
        
        using SHA256 sha256Hash = SHA256.Create();
        var hash = sha256Hash.ComputeHash(prevHashBytes.Concat(prevDataSignBytes).Concat(prevDataBytes).ToArray());
        var block = new BlockModel()
        {
            data = weightData,
            prevhash = BytesToHexString(hash),
            signature = BytesToHexString(sign.Item1)
        };
        var blockString = JsonSerializer.Serialize(block);
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync("http://89.108.115.118/nbc/newblock/", new StringContent(blockString, Encoding.UTF8, 
            "application/json"));
        var responseString = await response.Content.ReadAsStringAsync();

        await SendAuthor();
    }

    private async Task<BlockModel[]> GetBlocks()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://89.108.115.118/nbc/chain");
        var jsonResponse =  await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BlockModel[]>(jsonResponse);
    }
    
    private byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    private string BytesToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    private async Task SendAuthor()
    {
        var authorString = "Архипов Данил Дмитриевич 11-910";
        var authorModel = new AuthorModel()
        {
            autor = authorString,
            publickey = _publicKey,
            sign = BytesToHexString((await _dataSigner.SignData(Encoding.UTF8.GetBytes(authorString))).Item1),
        };
        var authorModelString = JsonSerializer.Serialize(authorModel);
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync("http://89.108.115.118/nbc/autor", new StringContent(authorModelString, Encoding.UTF8, 
            "application/json"));
        var responseString = await response.Content.ReadAsStringAsync();
    }
}