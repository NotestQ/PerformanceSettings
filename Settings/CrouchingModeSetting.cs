﻿using System.Collections.Generic;
using Zorro.Settings;
using HarmonyLib;
using PerformanceSettings.Settings.Type;

namespace PerformanceSettings.Settings
{
    public class CrouchingModeSetting : EnumSetting, IExposedSetting, IPatch
    {
        public override void ApplyValue()
        {
        }

        public override int GetDefaultValue()
        {
            return 0;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "Toggle", "Hold" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        public string GetDisplayName()
        {
            return "Crouching Mode";
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        internal class Patch
        {

            [HarmonyPatch(typeof(PlayerController), "MovementStateChanges")]
            [HarmonyPrefix]
            static void PatchCrouchingMode(PlayerController __instance)
            {
                var playerTraverse = Traverse.Create(__instance).Field("player");
                if (GameHandler.Instance.SettingsHandler.GetSetting<CrouchingModeSetting>().Value == 1 && playerTraverse != null)
                {
                    var player = playerTraverse.GetValue() as Player;
                    player.data.isCrouching = player.input.crouchIsPressed;
                    playerTraverse.SetValue(player);
                }
            }
        }
    }


}
