using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using Setting = Zorro.Settings.Setting;
using PerformanceSettings.Settings.Type;
using PerformanceSettings.Settings;
using PerformanceSettings;
using ContentSettings.API;
using UnityEngine.SceneManagement;
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
            var postProcessingSetting = new PostProcessingSetting();
            addSetting(postProcessingSetting);
            addSetting(new FOVSetting());
            addSetting(new ResetPerformanceToDefault());

            addSetting(new CrouchingModeSetting(), "CONTROLS-QOL");
            addSetting(new UseItemKeybindSetting(), "CONTROLS-QOL");
            addSetting(new AimKeybindSetting(), "CONTROLS-QOL");
            addSetting(new ResetControlsToDefault(), "CONTROLS-QOL");

            addSetting(new ResetGraphicsToDefault(), "GRAPHICS-QOL");
            addSetting(new ResetAudioToDefault(), "AUDIO-QOL");

            addPatches(new ShadowQualityPatch());
            addPatches(new VoiceVolumePatch());
            addPatches(new SFXVolumePatch());
            addPatches(new MasterVolumePatch());
            addPatches(new KeybindPatch());
            //addPatches(new ResetToDefault());

            harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            ApplyPatches();

            SceneManager.sceneLoaded += (_, _) => postProcessingSetting.ApplyValue();
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

        public static void addSetting(Setting setting, string tab = "PERFORMANCE", string? category = null)
        {
            SettingsLoader.RegisterSetting(tab, category, setting);
            additionalSettings.Add(setting);
        }

        internal static void addPatches(IPatch patch)
        {
            patches.Add(patch);
        }
    }
}
