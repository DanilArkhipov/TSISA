using BlockChain.Data.Entity;
using BlockChain.Domain;
using BlockChainConsoleApp.Utils;

namespace BlockChainConsoleApp.Commands;

public class AddBlockCommand
{
    private readonly String _data;

    public AddBlockCommand(string data)
    {
        _data = data;
    }

    public async Task Execute()
    {
        await using (var context = new BlockChainDbContext())
        {

            var lastBlock = context.Blocks
                .OrderByDescending(x => x.BlockId)
                .FirstOrDefault();
            Block newBlock;

            if (lastBlock is null)
            {
                newBlock = new Block(_data);
                context.Blocks.Add(newBlock);
                await context.SaveChangesAsync();
            }
            else
            {
                newBlock = new Block(_data, lastBlock);
                context.Blocks.Add(newBlock);
                await context.SaveChangesAsync();
            }
            
            Console.WriteLine("Добавлен блок:");
            BlockPrinter.PrintBLock(newBlock);
        }
    }
}