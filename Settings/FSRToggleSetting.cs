﻿using System.Collections.Generic;
using ContentSettings.API.Settings;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    public class FSRToggleSetting : EnumSetting, ICustomSetting
    {
        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            switch (base.Value)
            {
                case 0:
                    obj.upscalingFilter = UpscalingFilterSelection.Auto;
                    obj.fsrOverrideSharpness = false;
                    break;
                case 1:
                    obj.upscalingFilter = UpscalingFilterSelection.FSR;
                    obj.fsrOverrideSharpness = true;
                    break;
            }
        }

        public override int GetDefaultValue()
        {
            return 0;
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
            return "FSR 1.0 (FidelityFx Super Resolution)";
        }
    }
}
