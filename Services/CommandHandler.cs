using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace AcuthiaBot
{
    public class Initialize
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        // Asks if there is an existing CommandService and DiscordSocketClient
        //instance. If there are, retrieve them and add them to the DI container
        //If not, create our own
        public Initialize(CommandService commands = null, DiscordSocketClient client = null)
        {
            _commands = commands ?? new CommandService();
            _client = client ?? new DiscordSocketClient();
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            //.AddSingleton(new NotificationService)
            //.AddSingleton<DatabaseService>()
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
    }


    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;


        public CommandHandler (
            IServiceProvider services,
            DiscordSocketClient client, 
            CommandService commands) 
        {
            this._client = client;
            this._commands = commands;
            this._services = services;
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModuleAsync(
                assembly: Assembly.GetEntryAssembly(), 
                services: _services);
            _client.MessageReceived += OnMessageAsync;
        }

        private async Task OnMessageAsync(SocketMessage messageSent)
        {
            var message = messageSent as SocketUserMessage;
            int argPos = 0; // Create a number to track where the prefix ends and the command begins
            var context = new SocketCommandContext(_client, message); // Creates command context


            if (message == null) return; // Checks if message exists
            if (message.Author.Id == _client.CurrentUser.Id) return; // Checks if message sender == bot
            

            if ( !(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos) ) ) 
                return;

            await _commands.ExecuteAsync(
                context: context, 
                argPos: argPos, 
                services: _services);
        }
    }
}