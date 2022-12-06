using HarmonyLib;
using System.Collections.Generic;

namespace LimitCartographyPins.Patches
{

    [HarmonyPatch(typeof(Terminal))]
    class Terminal_Patches
    {


        [HarmonyPostfix]
        [HarmonyPatch("InitTerminal")]
        static void InitTerminal_Patch()
        {
            new Terminal.ConsoleCommand("removemypins", "Removes all personal pins from map", (args =>
            {
                RemoveMyPins(args.Context, args.Args);
            }), false);

            new Terminal.ConsoleCommand("removeotherspins", "Removes all pins from others from map", (args =>
            {
                RemoveOthersPins(args.Context, args.Args);
            }), false);

            new Terminal.ConsoleCommand("removeallpins", "Removes allpins from map", (args =>
            {
                RemoveAllPins(args.Context, args.Args);
            }), false);

            new Terminal.ConsoleCommand("writepindata", "Enables adding pins to cartography one time", (args =>
            {
                WritePinData(args.Context, args.Args);
            }), false);
        }



        public static void RemoveMyPins(Terminal context, string[] args)
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }
            int pinsDeleted = Minimap.m_instance.m_pins.Count;
            List<Minimap.PinData> pinsToKeep = new List<Minimap.PinData>();
            foreach (Minimap.PinData pin in Minimap.m_instance.m_pins)
            {

                if (!(pin.m_type == Minimap.PinType.Icon0
                    || pin.m_type == Minimap.PinType.Icon1
                    || pin.m_type == Minimap.PinType.Icon2
                    || pin.m_type == Minimap.PinType.Icon3
                    || pin.m_type == Minimap.PinType.Icon4)
                    || pin.m_ownerID != 0L)
                {
                    pinsToKeep.Add(pin);
                }
            }
            Minimap.m_instance.m_pins.Clear();
            foreach (Minimap.PinData pin in pinsToKeep)
            {
                Minimap.m_instance.m_pins.Add(pin);
            }
            pinsDeleted -= pinsToKeep.Count;
            context.AddString($"{pinsDeleted} pins deleted");
        }


        public static void RemoveOthersPins(Terminal context, string[] args)
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }
            int pinsDeleted = Minimap.m_instance.m_pins.Count;
            List<Minimap.PinData> pinsToKeep = new List<Minimap.PinData>();
            foreach (Minimap.PinData pin in Minimap.m_instance.m_pins)
            {

                if (!(pin.m_type == Minimap.PinType.Icon0
                    || pin.m_type == Minimap.PinType.Icon1
                    || pin.m_type == Minimap.PinType.Icon2
                    || pin.m_type == Minimap.PinType.Icon3
                    || pin.m_type == Minimap.PinType.Icon4)
                    || pin.m_ownerID == 0L)
                {
                    pinsToKeep.Add(pin);
                }
            }
            Minimap.m_instance.m_pins.Clear();
            foreach (Minimap.PinData pin in pinsToKeep)
            {
                Minimap.m_instance.m_pins.Add(pin);
            }
            pinsDeleted -= pinsToKeep.Count;
            context.AddString($"{pinsDeleted} pins deleted");
        }
        public static void RemoveAllPins(Terminal context, string[] args)
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }

            int pinsDeleted = Minimap.m_instance.m_pins.Count;
            List<Minimap.PinData> pinsToKeep = new List<Minimap.PinData>();
            foreach (Minimap.PinData pin in Minimap.m_instance.m_pins)
            {

                if (!(pin.m_type == Minimap.PinType.Icon0
                    || pin.m_type == Minimap.PinType.Icon1
                    || pin.m_type == Minimap.PinType.Icon2
                    || pin.m_type == Minimap.PinType.Icon3
                    || pin.m_type == Minimap.PinType.Icon4)
                    )
                {
                    pinsToKeep.Add(pin);
                }
            }
            Minimap.m_instance.m_pins.Clear();
            foreach (Minimap.PinData pin in pinsToKeep)
            {
                Minimap.m_instance.m_pins.Add(pin);
            }
            pinsDeleted -= pinsToKeep.Count;
            context.AddString($"{pinsDeleted} pins deleted");
        }
        public static void WritePinData(Terminal context, string[] args)
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }

            Minimap_Patches.WritePinDataOnce = true;
            context.AddString($"Pins will be written once on interact with cartography table");
        }
    }
}

