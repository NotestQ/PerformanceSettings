using HarmonyLib;
using PerformanceSettings.Settings.Type;
using Unity.Mathematics;

namespace PerformanceSettings.Settings
{
    internal class MasterVolumePatch:IPatch
    {
        [HarmonyPatch(typeof(MasterVolumeSetting), "GetMinMaxValue")]
        [HarmonyPostfix]
        static void PatchMasterVolumeMinMax(ref float2 __result)
        {
            __result = new float2(0f, 2f);
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(MasterVolumePatch));
        }
    }
}
