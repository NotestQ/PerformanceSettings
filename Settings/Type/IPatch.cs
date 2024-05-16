using HarmonyLib;

namespace PerformanceSettings.Settings.Type
{
    public interface IPatch
    {
        void ApplyPatch(ref Harmony harmony);
    }
}
