using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Rocket.Unturned.Chat;

namespace RocketmodTemplate.Commands
{
    public class CommandExample : IRocketCommand
    {
        public string Name => "example";
        public string Help => "A dog jump a fox";
        public string Syntax => "<syntax>";
        public List<string> Aliases => new List<string>() { "ex" };
        public List<string> Permissions => new List<string>() { "example" };
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedChat.Say("example");
        }
    }
}
