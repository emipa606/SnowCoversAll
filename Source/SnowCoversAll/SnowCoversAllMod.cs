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
    public static SnowCoversAllMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SnowCoversAllMod(ModContentPack content) : base(content)
    {
        Instance = this;
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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        var sandAddon = "";
        if (ModLister.OdysseyInstalled)
        {
            sandAddon = $"/{"SCA.sand".Translate()}";
        }

        Settings.SnowDepth =
            listingStandard.SliderLabeled("SCA.SnowDepth".Translate(Settings.SnowDepth.ToStringPercent(), sandAddon),
                Settings.SnowDepth, 0.05f, 1f);
        Settings.DeteriorationRate =
            listingStandard.SliderLabeled(
                "SCA.DeteriorationRate".Translate(Settings.DeteriorationRate.ToStringPercent(), sandAddon),
                Settings.DeteriorationRate, 0f, 1f);

        listingStandard.CheckboxLabeled("SCA.NotifyOnRecover".Translate(), ref Settings.NotifyOnRecover);
        if (Settings.NotifyOnRecover)
        {
            listingStandard.CheckboxLabeled("SCA.OnlyInHomeArea".Translate(), ref Settings.OnlyInHomeArea);
        }
        else
        {
            listingStandard.Gap();
        }

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("SCA.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}