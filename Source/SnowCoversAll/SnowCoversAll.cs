using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SnowCoversAll;

[StaticConstructorOnStartup]
public static class SnowCoversAll
{
    private static readonly Dictionary<Map, LostInSnow_MapComponent> mapComponents =
        new Dictionary<Map, LostInSnow_MapComponent>();

    public static readonly bool SandstormsLoaded;

    static SnowCoversAll()
    {
        SandstormsLoaded = ModsConfig.IsActive("Nightmare.Sandstorms");
        new Harmony("Mlie.SnowCoversAll").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static LostInSnow_MapComponent GetLostInSnowMapComponent(Map map)
    {
        if (mapComponents.TryGetValue(map, out var component))
        {
            return component;
        }

        component = map.GetComponent<LostInSnow_MapComponent>();
        mapComponents[map] = component;
        return component;
    }
}