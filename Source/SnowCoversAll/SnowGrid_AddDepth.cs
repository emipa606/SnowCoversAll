using HarmonyLib;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SnowGrid), nameof(SnowGrid.AddDepth))]
public static class SnowGrid_AddDepth
{
    public static void Postfix(SnowGrid __instance, IntVec3 c, float depthToAdd, Map ___map)
    {
        var currentDepth = __instance.GetDepth(c);
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);

        if (currentDepth > SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            if (currentDepth - depthToAdd > SnowCoversAllMod.instance.Settings.SnowDepth)
            {
                return;
            }

            mapComponent.LoseThings(c);
            return;
        }

        if (currentDepth - depthToAdd <= SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            return;
        }

        mapComponent.RecoverThings(c);
    }
}