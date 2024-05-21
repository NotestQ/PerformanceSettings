using System.Collections.Generic;
using ContentSettings.API.Settings;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    public class HDRSetting : EnumSetting, ICustomSetting
    {
        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            switch (base.Value)
            {
                case 0:
                    obj.supportsHDR = false;
                    break;
                case 1:
                    obj.supportsHDR = true;
                    break;
            }
        }

        public override int GetDefaultValue()
        {
            return 1;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "OFF", "ON" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "High Dynamic Range (HDR)";
        }
    }
}
