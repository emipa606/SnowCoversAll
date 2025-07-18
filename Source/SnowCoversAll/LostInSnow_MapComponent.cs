using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace SnowCoversAll;

public class LostInSnow_MapComponent : MapComponent
{
    private Dictionary<Thing, string> coveredByMaterials = new();
    private List<Thing> coveredByMaterialsKeys = [];
    private List<string> coveredByMaterialsValues = [];
    private Dictionary<Thing, IntVec3> forbiddenLostInSnow = new();
    private List<Thing> forbiddenLostInSnowKeys = [];
    private List<IntVec3> forbiddenLostInSnowValues = [];
    private Dictionary<Thing, IntVec3> lostInSnow = new();
    private List<Thing> lostInSnowKeys = [];
    private List<IntVec3> lostInSnowValues = [];

    public LostInSnow_MapComponent(Map map) : base(map)
    {
    }

    public void LoseThings(IntVec3 cell, string coveredBy = "snow")
    {
        var things = cell.GetThingList(map);
        if (!things.Any())
        {
            return;
        }

        if (things.Any(thing => thing.def.thingClass == typeof(Building_Storage)))
        {
            return;
        }

        // ReSharper disable once ForCanBeConvertedToForeach, things despawn in loop
        for (var index = 0; index < things.Count; index++)
        {
            var thing = things[index];

            if (thing.def.category != ThingCategory.Item)
            {
                continue;
            }

            if (thing.def.IsWithinCategory(ThingCategoryDefOf.Chunks) || thing.def == ThingDefOf.ChunkSlagSteel)
            {
                continue;
            }

            coveredByMaterials[thing] = coveredBy;

            if (thing.IsForbidden(Faction.OfPlayer))
            {
                forbiddenLostInSnow.Add(thing, thing.Position);
                thing.DeSpawn();
                continue;
            }

            lostInSnow.Add(thing, thing.Position);
            thing.DeSpawn();
        }
    }

    public void RecoverThings(IntVec3 cell, string coveredBy = "snow")
    {
        IEnumerable<Thing> thingsToRecover;
        if (lostInSnow.Any())
        {
            thingsToRecover = lostInSnow.Where(pair => pair.Value == cell).Select(pair => pair.Key).ToArray();
            if (thingsToRecover.Any())
            {
                for (var i = 0; i < thingsToRecover.Count(); i++)
                {
                    var thing = thingsToRecover.ElementAt(i);
                    if (coveredByMaterials.ContainsKey(thing))
                    {
                        if (coveredByMaterials[thing] != coveredBy)
                        {
                            continue;
                        }

                        coveredByMaterials.Remove(thing);
                    }

                    GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near);
                    lostInSnow.Remove(thing);
                    notifyAboutItem(thing, cell);
                }
            }
        }

        if (!forbiddenLostInSnow.Any())
        {
            return;
        }

        thingsToRecover = forbiddenLostInSnow.Where(pair => pair.Value == cell).Select(pair => pair.Key).ToArray();
        if (!thingsToRecover.Any())
        {
            return;
        }

        for (var i = 0; i < thingsToRecover.Count(); i++)
        {
            var thing = thingsToRecover.ElementAt(i);
            if (coveredByMaterials.ContainsKey(thing))
            {
                if (coveredByMaterials[thing] != coveredBy)
                {
                    continue;
                }

                coveredByMaterials.Remove(thing);
            }

            GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near);
            thing.SetForbidden(true, false);
            forbiddenLostInSnow.Remove(thing);
            notifyAboutItem(thing, cell, coveredBy);
        }
    }

    private void notifyAboutItem(Thing itemFound, IntVec3 cell, string coveredBy = "snow")
    {
        if (!SnowCoversAllMod.Instance.Settings.NotifyOnRecover)
        {
            return;
        }

        if (SnowCoversAllMod.Instance.Settings.OnlyInHomeArea && !map.areaManager.Home[cell])
        {
            return;
        }

        Messages.Message("SCA.RecoveryNotification".Translate(itemFound.Label, $"SCA.{coveredBy}".Translate()),
            itemFound,
            MessageTypeDefOf.PositiveEvent);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref lostInSnow, "lostInSnow", LookMode.Deep, LookMode.Value, ref lostInSnowKeys,
            ref lostInSnowValues);
        Scribe_Collections.Look(ref forbiddenLostInSnow, "forbiddenLostInSnow", LookMode.Deep, LookMode.Value,
            ref forbiddenLostInSnowKeys,
            ref forbiddenLostInSnowValues);
        Scribe_Collections.Look(ref coveredByMaterials, "coveredByMaterials", LookMode.Reference, LookMode.Value,
            ref coveredByMaterialsKeys,
            ref coveredByMaterialsValues);
    }

    public void DeteriorateCell(IntVec3 cell)
    {
        IEnumerable<KeyValuePair<Thing, IntVec3>> validItems = lostInSnow.Where(pair => pair.Value == cell).ToArray();

        if (validItems.Any())
        {
            foreach (var validItem in validItems)
            {
                if (deteriorateThing(validItem))
                {
                    lostInSnow.Remove(validItem.Key);
                }
            }
        }

        validItems = forbiddenLostInSnow.Where(pair => pair.Value == cell).ToArray();
        if (!validItems.Any())
        {
            return;
        }

        foreach (var validItem in validItems)
        {
            if (deteriorateThing(validItem))
            {
                forbiddenLostInSnow.Remove(validItem.Key);
            }
        }
    }

    private bool deteriorateThing(KeyValuePair<Thing, IntVec3> validThing)
    {
        if (!validThing.Key.def.CanEverDeteriorate)
        {
            return false;
        }

        if (ModsConfig.BiotechActive && validThing.Key is Genepack { Deteriorating: false })
        {
            return false;
        }

        var num = SteadyEnvironmentEffects.FinalDeteriorationRate(validThing.Key, false, true,
            validThing.Value.GetTerrain(map));
        if (num < 0.001f)
        {
            return false;
        }

        if (!Rand.Chance(num / 36f))
        {
            return false;
        }

        validThing.Key.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f));

        return validThing.Key.Destroyed;
    }
}