using System;
using Discord;
using Discord.WebSocket;
using System.Collections;

namespace AcuthiaBot
{
    public class Program
    {
	    public static Task Main(string[] args) => new Program().MainAsync(); //Turns MainAsync() into Main method

        private DiscordSocketClient? _client;

	    public async Task MainAsync()
	    {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            var token = File.ReadAllText("token.txt"); //Replace with env variable ASAP

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1); 
	    }

        private Task Log(LogMessage msg) //Logging method
        {
	            Console.WriteLine(msg.ToString());
	            return Task.CompletedTask;
        }
    }
}
