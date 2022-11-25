using BlockChain.Data.Entity;
using BlockChain.Domain;
using BlockChain.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlockChainConsoleApp.Commands;

public class VerifyBlocksCommand
{
    private readonly IDataSigner _dataSigner;

    public VerifyBlocksCommand()
    {
        _dataSigner = new DataSigner();
    }

    public async Task Execute()
    {
        await using (var context = new BlockChainDbContext())
        {
            var blocks = await context.Blocks
                .OrderBy(x => x.BlockId)
                .ToListAsync();


            Block prevBlock = null;
            for (int i = 0; i<blocks.Count - 1; i++)
            {
                if (!await blocks[i].Verify(prevBlock))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Цепочка неверна. Обнаружено несоответствие подписи хеша в блоке {blocks[i].BlockId} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                prevBlock = blocks[i];
            }
            Console.WriteLine("Цепочка корректна");
        }
    }
}