using BlockChain.Data.Entity;

namespace BlockChainConsoleApp.Utils;

public static class BlockPrinter
{
    public static void PrintBLock(Block block)
    {
        Console.WriteLine(
            $"\n\tId: {block.BlockId}\n\tData: {block.Data}\n\tDataSign: {Convert.ToBase64String(block.DataSign)}\n\tHash: {Convert.ToBase64String(block.Hash)}\n\tHashSign: {Convert.ToBase64String(block.HashSign)}");
    }
}