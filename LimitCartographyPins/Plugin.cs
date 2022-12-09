using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace LimitCartographyPins
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        internal const string ModName = "LimitCartographyPins";
        internal const string ModVersion = "1.0.1";
        internal const string Author = "Weiler";
        private const string ModGUID = Author + "." + ModName;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource ItemManagerModTemplateLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
        }
    }
}