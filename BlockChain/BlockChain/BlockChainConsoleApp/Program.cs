// See https://aka.ms/new-console-template for more information

using BlockChainConsoleApp.Commands;


string[] availableCommands = new string[]{"add-block", "print-blocks", "verify-block", "verify-blocks", "send-weights"};

if (args.Length == 0)
{
    Console.WriteLine("Необходимо передать команду");
    PrintAvailableCommands();
}
else
{
    switch (args[0])
    {
        case "add-block":
            await new AddBlockCommand(args.Length == 2 ? args[1] : "").Execute();
            break;
        case "print-blocks":
            await new PrintBlocksCommand().Execute();
            break;
        case "verify-block":
            await new VerifyBlockCommand(args.Length == 2 ? Convert.ToInt32(args[1]) : -1).Execute();
            break;
        case "verify-blocks":
            await new VerifyBlocksCommand().Execute();
            break;
        case "send-weights":
            await new SendWeightsCommand().Execute();
            break;
        default:
            Console.WriteLine("Не удалось распознать команду");
            PrintAvailableCommands();
            break;
    }
}



void PrintAvailableCommands()
{
    Console.WriteLine("Допустимые команды");
    foreach (var command in availableCommands)
    {
        Console.Write(command + " ");
    }
    Console.WriteLine();
}