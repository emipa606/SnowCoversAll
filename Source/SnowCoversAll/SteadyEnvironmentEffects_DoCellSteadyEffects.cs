using HarmonyLib;
using RimWorld;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SteadyEnvironmentEffects), "DoCellSteadyEffects")]
public static class SteadyEnvironmentEffects_DoCellSteadyEffects
{
    public static void Postfix(IntVec3 c, Map ___map)
    {
        if (!Rand.Chance(SnowCoversAllMod.instance.Settings.DeteriorationRate))
        {
            return;
        }

        SnowCoversAll.GetLostInSnowMapComponent(___map).DeteriorateCell(c);
    }
}