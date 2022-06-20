using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace DefaultLaserState
{
    public class Patch : NeosMod
    {
        public override string Name => "DefaultLaserState";
        public override string Author => "LeCloutPanda";
        public override string Version => "1.0.0";

        public static ModConfiguration config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> LASER_STATE = new ModConfigurationKey<bool>("Default Laser State", "", () => true);

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            config.Save(true);

            Harmony harmony = new Harmony($"net.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(CommonTool), "OnAwake")]
        class CommonToolPatch
        {
            [HarmonyPrefix]
            static void Prefix(CommonTool __instance, Sync<bool> ____laserEnabled)
            {
                __instance.RunInUpdates(3, () =>
                {
                    if (__instance.Slot.ActiveUser != __instance.LocalUser)
                        return;

                    ____laserEnabled.Value = config.GetValue(LASER_STATE);
                });
            }
        }
    }
}
