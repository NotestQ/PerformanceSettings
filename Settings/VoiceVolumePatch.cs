using HarmonyLib;
using PerformanceSettings.Settings.Type;
using Unity.Mathematics;

namespace PerformanceSettings.Settings
{
    internal class VoiceVolumePatch: IPatch
    {
        [HarmonyPatch(typeof(VoiceVolumeSetting), "GetMinMaxValue")]
        [HarmonyPostfix]
        static void PatchVoiceVolumeMinMax(ref float2 __result)
        {
            __result = new float2(0f, 2f);
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(VoiceVolumePatch));
        }
    }
}
