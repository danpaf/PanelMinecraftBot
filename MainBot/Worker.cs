using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Menus;
using DSharpPlus.SlashCommands;
using MainBot.Commands;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;


namespace MainBot;

public class Bot : BackgroundService
{
    private readonly DiscordClient _discord;
    private readonly IConfiguration _configuration;
    public CommandsNextExtension Commands { get; set; }
    private readonly string[] _adminRoles;
    private readonly string[] _ownerRoles;
    public Bot(IServiceScopeFactory scopeFactory
        , IConfiguration configuration)
    {
        _discord = new DiscordClient(new DiscordConfiguration
        {
            Token = configuration["token"],
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            
            
        });
        
        _configuration = configuration;

        _adminRoles = _configuration.GetSection("roles:adminRoles").Get<string[]>();
        _ownerRoles = _configuration.GetSection("roles:ownerRoles").Get<string[]>();

}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       
    
       _discord.UseInteractivity(new InteractivityConfiguration() 
        { 
            PollBehaviour = PollBehaviour.KeepEmojis,
            Timeout = TimeSpan.FromSeconds(30)
        });


        
        Dictionary<int, string> roles = new Dictionary<int, string>();
        roles = new Dictionary<int, string>()
        {
            [1] = "DEV",
            [2] = "admin",
            [3] = "help"
        };


        var ccfg = new CommandsNextConfiguration
        {
            StringPrefixes = new[] { _configuration["prefix"] },


            EnableDms = true,

            EnableMentionPrefix = true
        };
        this.Commands = this._discord.UseCommandsNext(ccfg);
        this.Commands.RegisterCommands<Moderating>();
        

        await _discord.ConnectAsync();
        await Task.Delay(-1);
    }
}