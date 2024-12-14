using HarmonyLib;
using Sandstorms;
using Verse;

namespace SnowCoversAll;

[HarmonyPatch(typeof(SandGrid), nameof(SandGrid.RemoveSand))]
public static class SandGrid_RemoveSand
{
    public static void Postfix(IntVec3 cell, Map ___map)
    {
        var mapComponent = SnowCoversAll.GetLostInSnowMapComponent(___map);
        mapComponent.RecoverThings(cell, "sand");
    }
}