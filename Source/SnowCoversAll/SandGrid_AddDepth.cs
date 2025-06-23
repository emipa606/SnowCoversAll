using HarmonyLib;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SandGrid), nameof(SandGrid.AddDepth))]
public static class SandGrid_AddDepth
{
    public static void Postfix(SandGrid __instance, IntVec3 c, float depthToAdd, Map ___map)
    {
        var currentDepth = __instance.GetDepth(c);
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);

        if (currentDepth > SnowCoversAllMod.Instance.Settings.SnowDepth)
        {
            if (currentDepth - depthToAdd > SnowCoversAllMod.Instance.Settings.SnowDepth)
            {
                return;
            }

            mapComponent.LoseThings(c, "sand");
            return;
        }

        if (currentDepth - depthToAdd <= SnowCoversAllMod.Instance.Settings.SnowDepth)
        {
            return;
        }

        mapComponent.RecoverThings(c, "sand");
    }
}