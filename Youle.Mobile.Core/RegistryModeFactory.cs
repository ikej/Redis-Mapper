using System;
using System.Collections.Generic;

namespace Youle.Mobile.Core
{
    public static class RegistryModeFactory
    {
        internal static List<Func<RegistryMode>> _conditions;
        internal static RegistryMode _currentMode;

        public static RegistryMode GetCurrentMode()
        {
            if (_conditions == null || _conditions.Count == 0)
            {
                if (_currentMode == RegistryMode.None)
                {
                    _currentMode = GetModeFromEnvConfig();
                }
            }
            else
            {
                _currentMode = GetModeFromConditions();
            }

            return _currentMode;
        }

        private static RegistryMode GetModeFromEnvConfig()
        {
            var mode = RegistryMode.None;
            if (ProjectConfigHelper.IsInDebugMode())
            {
                mode = RegistryMode.Debug;
            }
            else if (ProjectConfigHelper.IsInReleaseMode())
            {
                mode = RegistryMode.Release;
            }
            else if (ProjectConfigHelper.IsInLiveMode())
            {
                mode = RegistryMode.Live;
            }
            return mode;
        }

        private static RegistryMode GetModeFromConditions()
        {
            var mode = default(RegistryMode);

            if (_conditions != null)
            {
                foreach (var condition in _conditions)
                {
                    mode = mode | condition();
                }
            }

            return mode;
        }


        internal static void AddCondition(Func<RegistryMode> condition)
        {
            if (_conditions == null) _conditions = new List<Func<RegistryMode>>();

            _conditions.Add(condition);
        }

        internal static void ClearConditions()
        {
            if (_conditions != null) _conditions.Clear();
            _currentMode = RegistryMode.None;
        }

    }
}
