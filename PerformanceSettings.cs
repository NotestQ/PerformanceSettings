using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Settings;
using Setting = Zorro.Settings.Setting;
using PerformanceSettings.Settings.Type;
using PerformanceSettings.Settings;
using PerformanceSettings;
// ReSharper disable InconsistentNaming

namespace MoreSettings
{
    [ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, true)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class PerformanceSettings : BaseUnityPlugin 
    {
        public static PerformanceSettings Instance { get; private set; } = null!;
        private Harmony? harmony;
        internal static List<Setting> additionalSettings = new List<Setting>();
        internal static List<IPatch> patches = new List<IPatch>();
        private void Awake()
        {
            Instance = this;
            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has Loaded!!!!");

            addSetting(new HDRSetting());
            addSetting(new RenderScaleSetting());
            addSetting(new FSRToggleSetting());
            addSetting(new FSRSharpnessSetting());
            addSetting(new TextureResolutionSetting());
            addSetting(new AntiAliasingSetting());
            addSetting(new PostProcessingSetting());
            addSetting(new FOVSetting());
            addSetting(new CrouchingModeSetting());
            addSetting(new UseItemKeybindSetting());
            addSetting(new AimKeybindSetting());
            addSetting(new ResetGraphicsToDefault());
            addSetting(new ResetAudioToDefault());
            addSetting(new ResetControlsToDefault());

            addPatches(new ShadowQualityPatch());
            addPatches(new VoiceVolumePatch());
            addPatches(new SFXVolumePatch());
            addPatches(new MasterVolumePatch());
            addPatches(new KeybindPatch());
            //addPatches(new ResetToDefault());

            harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(MainPatch));
            ApplyPatches();
        }

        internal void ApplyPatches()
        {   
            foreach (var setting in additionalSettings)
            {
                if(setting is IPatch)
                {
                    var t = setting as IPatch;
                    t.ApplyPatch(ref harmony);
                }
            }

            foreach (var patch in patches)
            {
                patch.ApplyPatch(ref harmony);
            }
        }

        public static void addSetting(Setting setting)
        {
            additionalSettings.Add(setting);
        }

        internal static void addPatches(IPatch patch)
        {
            patches.Add(patch);
        }
    }

    internal class MainPatch
    {

        [HarmonyPatch(typeof(SettingsHandler),MethodType.Constructor)]
        [HarmonyPostfix]
        static void PatchSettingsHandler(SettingsHandler __instance)
        {
            var settings = Traverse.Create(__instance).Field("settings").GetValue() as List<Setting>;
            var settingsSaveLoad = Traverse.Create(__instance).Field("_settingsSaveLoad").GetValue() as ISettingsSaveLoad;
            settings = settings.Concat(PerformanceSettings.additionalSettings).ToList();
            Traverse.Create(__instance).Field("settings").SetValue(settings);
            foreach (Setting setting in PerformanceSettings.additionalSettings)
            {
                setting.Load(settingsSaveLoad);
                setting.ApplyValue();
            }
            Debug.Log("Settings Patch Applied [MoreSettings]");
        }

        [HarmonyPatch(typeof(Player),"Start")]
        [HarmonyPostfix]
        static void ApplySettingAtStart(Player __instance)
        {
            if(__instance.IsLocal == true)
            {
                Tools.ApplySettings();
            }
        }
    }
}
