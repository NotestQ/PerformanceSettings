﻿using HarmonyLib;
using PerformanceSettings.Settings.Type;
using System.Linq;
using Zorro.Settings;
using UnityEngine;
using TMPro;
using System.Reflection;
using ContentSettings.API.Settings;
using Zorro.UI;
using IntSetting = Zorro.Settings.IntSetting;

namespace PerformanceSettings.Settings
{
    internal class ResetGraphicsToDefault: StringSetting, ICustomSetting, IPatch
    {
        internal static TextMeshProUGUI titleComponent = null;
        internal static UIPageHandler pageHandler = null;
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Reset Graphics To Default";
        }

        public override void ApplyValue()
        {
        }

        protected override string GetDefaultValue()
        {
            return "";
        }

        internal static void OnButtonClicked()
        {
            var settings = GameHandler.Instance.SettingsHandler.GetSettings(SettingCategory.Graphics);
            foreach (Setting setting in settings)
            {

                if(setting is ResolutionSetting screenres)
                {
                    var res = screenres.GetResolutions().FirstOrDefault();
                    Debug.Log("Default Value " + res.width);
                    screenres.SetValue(res, GameHandler.Instance.SettingsHandler);
                    screenres.Update();
                }

                if(setting is IntSetting intset)
                {
                    MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    var res = dynMethod.Invoke(setting, new object[] { });
                    Debug.Log("Default Value " +(int)res);
                    intset.SetValue((int)res, GameHandler.Instance.SettingsHandler);
                    intset.Update();
                }

                if (setting is FloatSetting floatset)
                {
                    MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    var res = dynMethod.Invoke(setting, new object[] { });
                    Debug.Log("Default Value " + (float)res);
                    floatset.SetValue((float)res, GameHandler.Instance.SettingsHandler);
                    floatset.Update();
                }
            }

            GameHandler.Instance.SettingsHandler.Update();
            GameHandler.Instance.SettingsHandler.RegisterPage();
            if(pageHandler != null)
            {
                pageHandler.TransistionToPage<MainMenuMainPage>();
                pageHandler.TransistionToPage<MainMenuSettingsPage>();
            }
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        internal class Patch
        {
            [HarmonyPatch(typeof(KeyCodeSettingUI), "Setup")]
            [HarmonyPostfix]
            static void ResetGraphicsToDefaultSettingUI(KeyCodeSettingUI __instance, Setting setting, ISettingHandler settingHandler)
            {
                if (setting is ResetGraphicsToDefault keyCodeSetting)
                {
                    __instance.label.text = "Reset";
                    __instance.button.onClick.AddListener(ResetGraphicsToDefault.OnButtonClicked);
                }
            }

            [HarmonyPatch(typeof(SettingsCell), "Setup")]
            [HarmonyPostfix]
            static void GetSettingCellTitle(SettingsCell __instance, Setting setting) =>
                titleComponent = (setting is ResetGraphicsToDefault) ? __instance.title : titleComponent;

            [HarmonyPatch(typeof(MainMenuMainPage), "OnPageEnter")]
            [HarmonyPostfix]
            static void PatchGetSettingHandler(MainMenuMainPage __instance)=> pageHandler = __instance.GetPageHandler<UIPageHandler>();
        }
    }
}
