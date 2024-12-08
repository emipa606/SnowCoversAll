using Verse;

namespace SnowCoversAll;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class SnowCoversAllSettings : ModSettings
{
    public float DeteriorationRate;
    public bool NotifyOnRecover;
    public bool OnlyInHomeArea;
    public float SnowDepth = 0.5f;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref SnowDepth, "SnowDepth", 0.5f);
        Scribe_Values.Look(ref DeteriorationRate, "DeteriorationRate");
        Scribe_Values.Look(ref NotifyOnRecover, "NotifyOnRecover");
        Scribe_Values.Look(ref OnlyInHomeArea, "OnlyInHomeArea");
    }
}