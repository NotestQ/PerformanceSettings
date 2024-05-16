using System.Collections.Generic;
using Zorro.Settings;
using UnityEngine;

namespace PerformanceSettings.Settings
{
    public class TextureResolutionSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            QualitySettings.globalTextureMipmapLimit = 3 - base.Value;
        }

        public override int GetDefaultValue()
        {
            return 3;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "Low", "Medium", "High", "Very High" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Texture Resolution";
        }
    }
}
