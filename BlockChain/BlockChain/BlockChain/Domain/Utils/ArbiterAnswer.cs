using System.Text.Json.Serialization;

namespace BlockChain.Domain.Utils;

public class ArbiterAnswer
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("statusString")]
    public string StatusString { get; set; }
    
    [JsonPropertyName("timeStampToken")]
    public TimeStampTokenData TimeStampToken { get; set; }
}

public class TimeStampTokenData
{
    [JsonPropertyName("ts")]
    public string Ts { get; set; }
    
    [JsonPropertyName("signature")]
    public string Signature { get; set; }
}