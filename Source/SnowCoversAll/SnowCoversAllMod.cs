using Mlie;
using UnityEngine;
using Verse;

namespace SnowCoversAll;

[StaticConstructorOnStartup]
public class SnowCoversAllMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SnowCoversAllMod instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SnowCoversAllMod(ModContentPack content) : base(content)
    {
        instance = this;
        Settings = GetSettings<SnowCoversAllSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    public SnowCoversAllSettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Snow Covers All";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        var sandAddon = "";
        if (SnowCoversAll.SandstormsLoaded)
        {
            sandAddon = $"/{"SCA.sand".Translate()}";
        }

        Settings.SnowDepth =
            listing_Standard.SliderLabeled("SCA.SnowDepth".Translate(Settings.SnowDepth.ToStringPercent(), sandAddon),
                Settings.SnowDepth, 0.05f, 1f);
        Settings.DeteriorationRate =
            listing_Standard.SliderLabeled(
                "SCA.DeteriorationRate".Translate(Settings.DeteriorationRate.ToStringPercent(), sandAddon),
                Settings.DeteriorationRate, 0f, 1f);

        listing_Standard.CheckboxLabeled("SCA.NotifyOnRecover".Translate(), ref Settings.NotifyOnRecover);
        if (Settings.NotifyOnRecover)
        {
            listing_Standard.CheckboxLabeled("SCA.OnlyInHomeArea".Translate(), ref Settings.OnlyInHomeArea);
        }
        else
        {
            listing_Standard.Gap();
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SCA.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}