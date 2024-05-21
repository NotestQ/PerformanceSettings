using System.Collections.Generic;
using ContentSettings.API.Settings;
using UnityEngine.Rendering.Universal;
using Zorro.Settings;
using UnityEngine;


namespace PerformanceSettings.Settings
{
    public class AntiAliasingSetting : EnumSetting, ICustomSetting
    {
        public override void ApplyValue()
        {
            Camera[] cameras = (Camera[])Resources.FindObjectsOfTypeAll(typeof(Camera));
            foreach (var camera in cameras)
            {
                switch (base.Value)
                {
                    case 0:
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.None;
                        break;
                    case 1:
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                        camera.GetUniversalAdditionalCameraData().antialiasingQuality = AntialiasingQuality.High;
                        break;
                    case 2:
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                        camera.GetUniversalAdditionalCameraData().antialiasingQuality = AntialiasingQuality.High;
                        break;
                }
            }
        }

        public override int GetDefaultValue()
        {
            return 0;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "OFF", "FXAA", "SMAA" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Anti Aliasing";
        }
    }
}
