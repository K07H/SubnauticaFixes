using System;
using UnityEngine;

namespace SubnauticaFixes
{
    /// <summary>
    /// Will fix the signs loading bug (see https://youtu.be/8eGj40Xzkag).
    /// This bug happens in vanilla version of the game (no mods installed).
    /// Note that the initial exceptions will still be raised upon loading (and logged in Player.log file).
    /// That's because this fix is applied as a postfix method (in other words, signs are fixed after their initial loading failure).
    /// </summary>
    public class uGUI_SignInputFixer
    {
        /// <summary>
        /// This function is patched into the game using Harmony.
        /// </summary>
        /// <param name="__instance">The object instance that owns the original method.</param>
        public static void UpdateScale_Postfix(uGUI_SignInput __instance)
        {
            // If this uGUI_SignInput is enabled and attached to a game object.
            if (__instance.enabled && __instance.gameObject != null && __instance.gameObject.GetComponent<SignFixerComponent>() == null)
            {
                // If this uGUI_SignInput has a valid parent.
                if (__instance.transform != null && __instance.transform.parent != null)
                {
                    // Get Sign component from this uGUI_SignInput parent.
                    Sign sign = __instance.transform.parent.GetComponent<Sign>();
                    // If we were able to get the Sign component for this uGUI_SignInput.
                    if (sign != null)
                    {
                        // Add our fixer comonent to the game object.
                        __instance.gameObject.AddComponent<SignFixerComponent>();
                    }
                }
            }
        }
    }

    /// <summary>
    /// MonoBehaviour component attached to sign objects to fix their loading problem.
    /// </summary>
    public class SignFixerComponent : MonoBehaviour
    {
        /// <summary>
        /// This function is called by <see cref="Awake"/> method.
        /// </summary>
        private void RestoreSignState()
        {
            // If current MonoBehaviour is enabled and if this component has a valid parent.
            if (enabled && transform != null && transform.parent != null)
            {
                // Get the Sign component from parent.
                Sign sign = transform.parent.GetComponent<Sign>();
                // If we were able to get the Sign component.
                if (sign != null)
                    sign.OnProtoDeserialize(null); // Restore state again now that RectTransform (base->uGUI_SignInput.rt) is set.
            }
        }

        /// <summary>
        /// This function gets called once the MonoBehaviour wakes up.
        /// </summary>
        public void Awake()
        {
            // If current MonoBehavour is enabled, call RestoreSignState next frame (we can't restore the Sign state this frame because the RectTransform property of Sign's base class uGUI_SignInput has not been set yet). 
            if (enabled)
                Invoke("RestoreSignState", 1f);
        }
    }
}
