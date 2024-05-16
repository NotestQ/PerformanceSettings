using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    public class FSRSharpnessSetting : FloatSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "FSR Sharpness Amount";
        }

        public override float GetDefaultValue()
        {
            return 0.92f;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(0f, 1f);
        }

        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            obj.fsrSharpness = base.Value;
        }
    }
}
