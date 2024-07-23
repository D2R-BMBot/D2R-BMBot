namespace ScriptEngine;

public class MonitorParameters(int p_maxHealth, 
                               int p_healthPotionThreshold, 
                               int p_manaPotionThreshold, 
                               int p_rejuvenationPotionThreshold, 
                               int p_townPortalThreshold, 
                               int p_leaveGameThreshold)
{
    public int MaxHealth                      { get; set; } = p_maxHealth;
    public int UseHealthPotionThreshold       { get; set; } = p_healthPotionThreshold;
    public int UseManaPotionThreshold         { get; set; } = p_manaPotionThreshold;
    public int TownPortalHealthThreshold      { get; set; } = p_townPortalThreshold;
    public int LeaveGameHealthThreshold       { get; set; } = p_leaveGameThreshold;
}