using HarmonyLib;
using UnityEngine;

namespace MoreSettings
{
    public class Tools
    {

        public static void LogMessage(string message)
        {
            FileLog.Log(message);
            Debug.Log(message);
        }

        public static void ApplySettings()
        {
            foreach (var setting in GameHandler.Instance.SettingsHandler.GetAllSettingsNonAlloc())
            {
                setting.ApplyValue();
            }
        }
    }
}
