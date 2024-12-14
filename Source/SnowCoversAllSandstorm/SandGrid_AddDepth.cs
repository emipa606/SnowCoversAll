using HarmonyLib;
using Sandstorms;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SandGrid), nameof(SandGrid.AddDepth))]
public static class SandGrid_AddDepth
{
    public static void Postfix(SandGrid __instance, IntVec3 cell, float amount, Map ___map)
    {
        var currentDepth = __instance.GetDepth(cell);
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);

        if (currentDepth > SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            if (currentDepth - amount > SnowCoversAllMod.instance.Settings.SnowDepth)
            {
                return;
            }

            mapComponent.LoseThings(cell, "sand");
            return;
        }

        if (currentDepth - amount <= SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            return;
        }

        mapComponent.RecoverThings(cell, "sand");
    }
}