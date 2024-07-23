using System.Threading.Channels;

namespace ScriptEngine;

public class MonitorScript(ChannelWriter<ScriptCommand> p_commandInjector,
                           MonitorParameters            p_parameters)
{
    public async Task RunScript(CancellationToken p_cancellationToken)
    {
        while (!p_cancellationToken.IsCancellationRequested)
        {
            var currentCharacterValues = GetCurrentCharacterStats();
            
            var characterNeedsToLeaveGame = currentCharacterValues.Health < p_parameters.LeaveGameHealthThreshold;
            var characterNeedsToLeaveForTown = currentCharacterValues.Health < p_parameters.TownPortalHealthThreshold;
            var characterNeedsHealing = currentCharacterValues.Health < p_parameters.UseHealthPotionThreshold;
            var characterNeedsMana    = currentCharacterValues.Mana < p_parameters.UseManaPotionThreshold;
            
            if (characterNeedsToLeaveGame)
            {
                await p_commandInjector.WriteAsync(new ScriptCommand("LeaveGame"), p_cancellationToken);
            }
            else if (characterNeedsToLeaveForTown)
            {
                await p_commandInjector.WriteAsync(new ScriptCommand("UseTownPortal"), p_cancellationToken);
            }
            
            if (characterNeedsHealing)
            {
                await p_commandInjector.WriteAsync(new ScriptCommand("UseHealthPotion"), p_cancellationToken);
            }
            else if (characterNeedsMana)
            {
                await p_commandInjector.WriteAsync(new ScriptCommand("UseManaPotion"), p_cancellationToken);
            }
        }
    }

    private CharacterStats GetCurrentCharacterStats()
    {
        throw new NotImplementedException();
    }
}