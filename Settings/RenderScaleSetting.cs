using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    public class RenderScaleSetting : FloatSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Render Scale";
        }

        public override float GetDefaultValue()
        {
            return 1f;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(0.25f, 1f);
        }

        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            obj.renderScale = base.Value;
        }
    }
}
