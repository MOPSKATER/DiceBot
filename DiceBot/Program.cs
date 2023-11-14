using Discord.WebSocket;
using Discord;
using Discord.Net;
using Newtonsoft.Json;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync(args[0]);


    private DiscordSocketClient? _client;

    public async Task MainAsync(string token)
    {
        _client = new DiscordSocketClient();
        var guild = _client.GetGuild(1173980055387512902);

        _client.Log += Log;
        SlashCommandBuilder rollCommandBuilder = new SlashCommandBuilder()
            .WithName("r")
            .WithDescription("Rolls a dice (5d10 rolls 5 d10 dices)");


        _client.MessageReceived += MsgRcv;

        await _client.LoginAsync(TokenType.Bot, token);

        try
        {
            await guild.CreateApplicationCommandAsync(rollCommandBuilder.Build());
        }
        catch (HttpException exception)
        {
            Console.WriteLine(exception.ToString());
        }

        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private Task MsgRcv(SocketMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}