using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace SnowCoversAll;

public class LostInSnow_MapComponent : MapComponent
{
    private Dictionary<Thing, IntVec3> forbiddenLostInSnow = new Dictionary<Thing, IntVec3>();
    private List<Thing> forbiddenLostInSnowKeys = [];
    private List<IntVec3> forbiddenLostInSnowValues = [];
    private Dictionary<Thing, IntVec3> lostInSnow = new Dictionary<Thing, IntVec3>();
    private List<Thing> lostInSnowKeys = [];
    private List<IntVec3> lostInSnowValues = [];

    public LostInSnow_MapComponent(Map map) : base(map)
    {
    }

    public void LoseThings(IntVec3 cell)
    {
        var things = cell.GetThingList(map);
        if (!things.Any())
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

            if (thing.HasThingCategory(ThingCategoryDefOf.Chunks) || thing.def == ThingDefOf.ChunkSlagSteel)
            {
                continue;
            }

            if (thing.IsForbidden(Faction.OfPlayer))
            {
                thing.DeSpawn();
                forbiddenLostInSnow.Add(thing, thing.Position);
                continue;
            }

            thing.DeSpawn();
            lostInSnow.Add(thing, thing.Position);
        }
    }

    public void RecoverThings(IntVec3 cell)
    {
        if (!lostInSnow.Any())
        {
            return;
        }

        var thingsToRecover = lostInSnow.Where(pair => pair.Value == cell).Select(pair => pair.Key);
        if (thingsToRecover.Any())
        {
            for (var i = 0; i < thingsToRecover.Count(); i++)
            {
                var thing = thingsToRecover.ElementAt(i);
                GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near);
                lostInSnow.Remove(thing);
                notifyAboutItem(thing, cell);
            }
        }

        thingsToRecover = forbiddenLostInSnow.Where(pair => pair.Value == cell).Select(pair => pair.Key);
        if (!thingsToRecover.Any())
        {
            return;
        }

        for (var i = 0; i < thingsToRecover.Count(); i++)
        {
            var thing = thingsToRecover.ElementAt(i);
            GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near);
            thing.SetForbidden(true, false);
            forbiddenLostInSnow.Remove(thing);
            notifyAboutItem(thing, cell);
        }
    }

    private void notifyAboutItem(Thing itemFound, IntVec3 cell)
    {
        if (!SnowCoversAllMod.instance.Settings.NotifyOnRecover)
        {
            return;
        }

        if (SnowCoversAllMod.instance.Settings.OnlyInHomeArea && !map.areaManager.Home[cell])
        {
            return;
        }

        Messages.Message("SCA.RecoveryNotification".Translate(itemFound.Label), itemFound,
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
    }
}