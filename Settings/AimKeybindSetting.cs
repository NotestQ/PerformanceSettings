using ContentSettings.API.Settings;
using UnityEngine;
using Zorro.Settings;

namespace PerformanceSettings.Settings
{
    internal class AimKeybindSetting : KeyCodeSetting, ICustomSetting
    {

        public string GetDisplayName()
        {
            return "Aim";
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        public override KeyCode GetDefaultKey()
        {
            return KeyCode.Mouse1;
        }
    }
}
