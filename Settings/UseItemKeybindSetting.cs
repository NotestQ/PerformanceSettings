using HarmonyLib;
using PerformanceSettings.Settings.Type;
using UnityEngine;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    internal class UseItemKeybindSetting : KeyCodeSetting, IExposedSetting, IPatch
    {
        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        public string GetDisplayName()
        {
            return "Use Item";
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        public override KeyCode GetDefaultKey()
        {
            return KeyCode.Mouse0;
        }
    }
}
