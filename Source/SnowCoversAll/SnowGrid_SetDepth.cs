using HarmonyLib;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SnowGrid), nameof(SnowGrid.SetDepth))]
public static class SnowGrid_SetDepth
{
    public static void Prefix(out float __state, SnowGrid __instance, IntVec3 c)
    {
        __state = __instance.GetDepth(c);
    }

    public static void Postfix(float __state, IntVec3 c, float newDepth, Map ___map)
    {
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);

        if (__state > SnowCoversAllMod.instance.Settings.SnowDepth &&
            newDepth < SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            mapComponent.RecoverThings(c);
            return;
        }

        if (__state < SnowCoversAllMod.instance.Settings.SnowDepth &&
            newDepth > SnowCoversAllMod.instance.Settings.SnowDepth)
        {
            mapComponent.LoseThings(c);
        }
    }
}