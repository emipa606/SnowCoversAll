using HarmonyLib;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SandGrid), nameof(SandGrid.SetDepth))]
public static class SandGrid_SetDepth
{
    public static void Prefix(out float __state, SandGrid __instance, IntVec3 c)
    {
        __state = __instance.GetDepth(c);
    }

    public static void Postfix(float __state, IntVec3 c, float newDepth, Map ___map)
    {
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);

        if (__state > SnowCoversAllMod.Instance.Settings.SnowDepth &&
            newDepth < SnowCoversAllMod.Instance.Settings.SnowDepth)
        {
            mapComponent.RecoverThings(c, "sand");
            return;
        }

        if (__state < SnowCoversAllMod.Instance.Settings.SnowDepth &&
            newDepth > SnowCoversAllMod.Instance.Settings.SnowDepth)
        {
            mapComponent.LoseThings(c, "sand");
        }
    }
}