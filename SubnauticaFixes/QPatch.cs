using Harmony;
using System;
using System.Reflection;

namespace SubnauticaFixes
{
    public class QPatch
    {
        private static HarmonyInstance HarmonyInstance = null;

        public static void Patch()
        {
            // Log patching start.
            Console.WriteLine("Applying Subnautica fixes.");
            try
            {
                // Initialize Harmony.
                HarmonyInstance = HarmonyInstance.Create("com.osubmarin.subnauticafixes");

                // Fix Sign loading problem (see https://youtu.be/8eGj40Xzkag).
                var updateScaleMethod = typeof(uGUI_SignInput).GetMethod("UpdateScale", BindingFlags.NonPublic | BindingFlags.Instance);
                var updateScalePostfix = typeof(uGUI_SignInputFixer).GetMethod("UpdateScale_Postfix", BindingFlags.Public | BindingFlags.Static);
                HarmonyInstance.Patch(updateScaleMethod, null, new HarmonyMethod(updateScalePostfix));

                // Log patching success.
                Console.WriteLine("Subnautica fixes applied successfully.");
            }
            catch (Exception e)
            {
                // Log patching error.
                Console.WriteLine("An error happened while applying Subnautica fixes. Exception=[" + e.ToString() + "]");
            }
        }
    }
}
