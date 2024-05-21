using System;
using System.Collections.Generic;
using HarmonyLib;
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
    internal class ResetPerformanceToDefault : StringSetting, ICustomSetting, IPatch
    {
        internal static TextMeshProUGUI titleComponent = null;

        public string GetDisplayName()
        {
            return "Reset Performance Tab To Default";
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
            ContentSettings.API.SettingsLoader.TryGetTab("PERFORMANCE", out Dictionary<string, List<Setting>> settingsByCategory);
            foreach (KeyValuePair<string, List<Setting>> keyValuePair in settingsByCategory)
            {
                foreach (Setting setting in keyValuePair.Value)
                {
                    Debug.Log($"!!!!!!!!!!!!!!!! {setting.ToString()}");
                    if (setting is ResolutionSetting screenres)
                    {
                        var res = screenres.GetResolutions().FirstOrDefault();
                        Debug.Log("Default Value " + res.width);
                        screenres.SetValue(res, GameHandler.Instance.SettingsHandler);
                        screenres.Update();
                    }

                    if (setting is EnumSetting enumset)
                    {
                        MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.Public | BindingFlags.Instance);
                        var res = dynMethod.Invoke(setting, new object[] { });
                        Debug.Log("Default Value " + (int)res);
                        enumset.SetValue((int)res, GameHandler.Instance.SettingsHandler);
                        enumset.Update();
                        continue;
                    }

                    if (setting is IntSetting intset)
                    {
                        MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
                        var res = dynMethod.Invoke(setting, new object[] { });
                        Debug.Log("Default Value " + (int)res);
                        intset.SetValue((int)res, GameHandler.Instance.SettingsHandler);
                        intset.Update();
                    }

                    if (setting is FloatSetting floatset)
                    {
                        MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var res = dynMethod.Invoke(setting, new object[] { });
                        Debug.Log("Default Value " + (float)res);
                        floatset.SetValue((float)res, GameHandler.Instance.SettingsHandler);
                        floatset.Update();
                    }
                }
            }

            //GameHandler.Instance.SettingsHandler.Update();
            //GameHandler.Instance.SettingsHandler.RegisterPage();
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
                if (setting is ResetPerformanceToDefault keyCodeSetting)
                {
                    __instance.label.text = "Reset";
                    __instance.button.onClick.AddListener(ResetPerformanceToDefault.OnButtonClicked);
                }
            }

            [HarmonyPatch(typeof(SettingsCell), "Setup")]
            [HarmonyPostfix]
            static void GetSettingCellTitle(SettingsCell __instance, Setting setting) =>
                titleComponent = (setting is ResetPerformanceToDefault) ? __instance.title : titleComponent;
        }
    }
}
