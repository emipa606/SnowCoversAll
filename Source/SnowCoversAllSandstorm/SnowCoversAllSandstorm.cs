using System.Reflection;
using HarmonyLib;
using Verse;

namespace SnowCoversAllSandstorm;

[StaticConstructorOnStartup]
public static class SnowCoversAllSandstorm
{
    static SnowCoversAllSandstorm()
    {
        new Harmony("Mlie.SnowCoversAllSandstorm").PatchAll(Assembly.GetExecutingAssembly());
        Log.Message("[SnowCoversAll]: Adding compatibility with Sandstorms");
    }
}