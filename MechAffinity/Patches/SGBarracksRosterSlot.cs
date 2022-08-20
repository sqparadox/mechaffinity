﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using BattleTech.UI.Tooltips;
using Harmony;
using MechAffinity;
using MechAffinity.Data;

namespace MechAffinity.Patches
{
    [HarmonyPatch(typeof(SGBarracksRosterSlot), "Refresh")]
    public static class SGBarracksRosterSlot_Refresh_Patch
    {
        public static void Postfix(SGBarracksRosterSlot __instance, UIColorRefTracker ___pilotTypeBackground)
        {
            if (__instance.Pilot == null)
                return;

            HBSTooltip tooltip = __instance.gameObject.GetComponent<HBSTooltip>() ?? __instance.gameObject.AddComponent<HBSTooltip>();

            Pilot pilot = __instance.Pilot;
            string Desc = tooltip.GetText();
            if (String.IsNullOrEmpty(Desc))
            {
                Desc = "";
            }
            
            foreach (PilotTooltipTag pqTag in Main.settings.quirkSettings.tooltipTags)
            {
                if (pilot.pilotDef.PilotTags.Contains(pqTag.tag))
                {
                    Desc += $"{pqTag.tooltipText}\n\n";
                }
            }

            PilotUiManager.Instance.SetPilotIcon(pilot, ___pilotTypeBackground);

            if (Main.settings.enablePilotQuirks)
            {
                Desc += PilotQuirkManager.Instance.getPilotToolTip(pilot);
            }

            if (Main.settings.enablePilotAffinity)
            {
                Desc += "<b>Pilot Affinities:</b>\n\n";
                Desc += PilotAffinityManager.Instance.getPilotToolTip(pilot);
            }

            var descriptionDef = new BaseDescriptionDef("Tags", pilot.Callsign, Desc, null);
            tooltip.SetDefaultStateData(TooltipUtilities.GetStateDataFromObject(descriptionDef));
        }
    }
}
