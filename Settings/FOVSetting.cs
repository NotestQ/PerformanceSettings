using System;
using ContentSettings.API.Settings;
using Unity.Mathematics;
using Zorro.Settings;
using PerformanceSettings.Settings.Type;
using HarmonyLib;
using UnityEngine;

namespace PerformanceSettings.Settings
{
    public class FOVSetting : FloatSetting, ICustomSetting, IPatch
    {
        static Traverse<float> baseFOVTraverse = null;

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "FOV (Field Of View)";
        }

        public override float GetDefaultValue()
        {
            return 70.08072f;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(60f, 100f);
        }

        public override void ApplyValue()
        {
            if (baseFOVTraverse != null)
            {
                baseFOVTraverse.Value = Value;
            }
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        internal class Patch
        {
            [HarmonyPatch(typeof(MainCamera),"Awake")]
            [HarmonyPostfix]
            static void PatchFOV()
            {
                baseFOVTraverse = Traverse.Create(MainCamera.instance).Field<float>("baseFOV");
                Debug.Log("traverse " + baseFOVTraverse);
                try
                {
                    // unknown error at start, ignore it
                    
                    if (baseFOVTraverse?.Value != null) baseFOVTraverse.Value = ContentSettings.API.SettingsLoader.GetSetting<FOVSetting>()!.Value;
                }
                catch (Exception e) { }
            }
        }
    }
}
