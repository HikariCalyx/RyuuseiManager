using System;

namespace RyuuseiManager.API
{
    public class WineCheck
    {
        public static bool IsRunningUnderWine()
        {
            string wineprefix = Environment.GetEnvironmentVariable("WINEPREFIX") ?? "";
            return !string.IsNullOrEmpty(wineprefix);
        }
    }

}