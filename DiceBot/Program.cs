using Discord.WebSocket;
using Discord;
using Discord.Net;
using System.Diagnostics;
using DiceBot;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync(args[0]);


    private DiscordSocketClient _client = new();
    private Dicer _dicer = new();

    public async Task MainAsync(string token)
    {
        _client.Ready += Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task Ready()
    {
        var guild = _client.GetGuild(1173980055387512902);

        SlashCommandBuilder rollCommandBuilder = new SlashCommandBuilder()
            .WithName("r")
            .WithDescription("Rolls a dice (5d10 rolls 5 d10 dices)")
            .AddOption("dice", ApplicationCommandOptionType.String, "<Dice amount>d<Dice type>", isRequired: true);

        SlashCommandBuilder eRollCommandBuilder = new SlashCommandBuilder()
            .WithName("er")
            .WithDescription("Ephemeral roll only responds the command sender")
            .AddOption("dice", ApplicationCommandOptionType.String, "<Dice amount>d<Dice type>", isRequired: true);

        try
        {
            await guild.CreateApplicationCommandAsync(rollCommandBuilder.Build());
            await guild.CreateApplicationCommandAsync(eRollCommandBuilder.Build());
            //await _client.CreateGlobalApplicationCommandAsync(rollCommandBuilder.Build());
            //await _client.CreateGlobalApplicationCommandAsync(eRollCommandBuilder.Build());
        }
        catch (HttpException exception)
        {
            Console.WriteLine(exception.ToString());
            Environment.Exit(1);
        }
        Console.WriteLine("Bot ready");
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "r": 
                await RollDices(command, false);
                break;
            case "er":
                await RollDices(command, true);
                break;
        }
    }

    private async Task RollDices(SocketSlashCommand command, bool ephemeral)
    {
        string diceString = (string)command.Data.Options.First().Value;
        await command.RespondAsync(_dicer.RollDice(diceString), ephemeral: ephemeral);
    }
}