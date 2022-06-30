using Discord.Commands;

namespace AcuthiaBot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public HelpModule(CommandService service)
        {
            this._service = service;
        }

        [Command("help")]
        [Summary("Lists all commands")]
        public async Task HelpAsync()
        {
            
        }
    }
}