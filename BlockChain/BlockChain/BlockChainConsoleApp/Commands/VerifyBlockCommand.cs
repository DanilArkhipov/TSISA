using BlockChain.Data.Entity;
using BlockChain.Domain;
using BlockChainConsoleApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlockChainConsoleApp.Commands;

public class VerifyBlockCommand
{
    private readonly int _blockId;

    public VerifyBlockCommand(int blockId)
    {
        _blockId = blockId;
    }

    public async Task Execute()
    {
        await using (var context = new BlockChainDbContext())
        {
            var block = await context.Blocks
                .FirstOrDefaultAsync(x=> x.BlockId == _blockId);

            if (block == null)
            {
                Console.WriteLine("Указан неверный номер блока");
                return;
            }
            
            var prevBlock = await context.Blocks
                .FirstOrDefaultAsync(x=> x.BlockId == _blockId - 1);

            var verificationResult = await block.Verify(prevBlock);
            if (verificationResult)
            {
                Console.WriteLine($"Данные в блоке с номером {_blockId} корректны");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Данные в блоке с номером {_blockId} некорректны");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}