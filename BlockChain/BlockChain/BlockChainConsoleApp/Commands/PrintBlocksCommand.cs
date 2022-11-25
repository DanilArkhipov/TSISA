using BlockChain.Data.Entity;
using BlockChain.Domain;
using BlockChainConsoleApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlockChainConsoleApp.Commands;

public class PrintBlocksCommand
{
    public async Task Execute()
    {
        await using (var context = new BlockChainDbContext())
        {

            var blocks = await context.Blocks
                .ToListAsync();

            Console.WriteLine("Блоки:");
            foreach (var block in blocks)
            {
                BlockPrinter.PrintBLock(block);
            }

        }
    }
}