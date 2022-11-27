namespace BlockChainConsoleApp;

public class BlockModel
{
    public string prevhash { get; set; }
    
    public NeuralWeightsModel data { get; set; }
    
    public string signature { get; set; }
}