using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static Minimap;

namespace LimitCartographyPins.Patches
{
    [HarmonyPatch(typeof(Minimap))]
    class Minimap_Patches
    {
        public static bool WritePinDataOnce = false;
        static List<Minimap.PinData> pins = new List<PinData>();

        [HarmonyPrefix]
        [HarmonyPatch("GetSharedMapData")]
        static void GetSharedMapData_PrefixPatch(ref List<Minimap.PinData> ___m_pins, byte[] oldMapData)
        {


            /* VALHEIM DEFAULT BEHAVIOR
             * writes cartography explored map with your explored map
             * reads YOUR pins
             * writes ONLY YOUR pins to table
             * pins not in your map is therefor removed by default!
             * 
             * As we want pin write on demand, we need to read existing pins from the byte array, then merge yours IF yours are suppose to be added
             * algorithm for reading package is copied from the games method as it needs to match
             * 
             */


            List<Minimap.PinData> pinsOnTable = new List<Minimap.PinData>();
            //read oldMapData Package
            ZPackage zPackage = new ZPackage(oldMapData);
            int num = zPackage.ReadInt();
            List<bool> list = Minimap.instance.ReadExploredArray(zPackage, num);
            if (list == null)
            {
                //return false; we dont care about explored, only pins are overwritten
            }
            if (num >= 2)
            {
                long playerID = Player.m_localPlayer.GetPlayerID();
                int num3 = zPackage.ReadInt();
                for (int k = 0; k < num3; k++)
                {
                    Minimap.PinData pin = new PinData();
                    //pindata
                    long num4 = zPackage.ReadLong();
                    string text = zPackage.ReadString();
                    Vector3 pos = zPackage.ReadVector3();
                    PinType type = (PinType)zPackage.ReadInt();
                    bool isChecked = zPackage.ReadBool();

                    pin.m_ownerID = num4;
                    pin.m_name = text;
                    pin.m_pos = pos;
                    pin.m_type = type;
                    pin.m_checked = isChecked;
                    pin.m_save = true;
                    if (!HavePinInRange(pinsOnTable, pin.m_pos, 1f))
                        pinsOnTable.Add(pin);
                }
            }








            //reset own cache
            pins.Clear();

            //store everything in own cache
            foreach (Minimap.PinData pin in ___m_pins)
            {
                pins.Add(pin);
            }

            //store non-player pins (preserves pins like bosses), death pins are removed in the original valheim code
            List<Minimap.PinData> pinsToKeep = new List<Minimap.PinData>();
            foreach (Minimap.PinData pin in ___m_pins)
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

            //remove all own pins
            ___m_pins.Clear();

            //always add maptable pins
            foreach (Minimap.PinData pin in pinsOnTable)
            {
                if (!HavePinInRange(___m_pins, pin.m_pos, 1f))
                    ___m_pins.Add(pin);
            }

            //add non-player pins, if adding own pins is enabled
            if (WritePinDataOnce)
            {
                foreach (Minimap.PinData pin in pinsToKeep)
                {
                    if (!HavePinInRange(___m_pins, pin.m_pos, 1f))
                        ___m_pins.Add(pin);
                }
            }





        }

        [HarmonyPostfix]
        [HarmonyPatch("GetSharedMapData")]
        static void GetSharedMapData_PostfixPatch(ref List<Minimap.PinData> ___m_pins)
        {


            //remove all own pins (this is currently merged with maptables pins!)
            ___m_pins.Clear();

            //add all cached pins
            foreach (Minimap.PinData pin in pins)
            {
                ___m_pins.Add(pin);
            }

            //empty cache
            pins.Clear();


            if (WritePinDataOnce)
            {
                WritePinDataOnce = false;
            }
        }

        //method copied from valheim, but adding reference list of pins as parameter, as we want to check different list than players own list
        private static bool HavePinInRange(List<Minimap.PinData> referencePins, Vector3 pos, float radius)
        {
            foreach (Minimap.PinData pin in referencePins)
            {
                if (pin.m_save && Utils.DistanceXZ(pos, pin.m_pos) < radius)
                {
                    return true;
                }
            }
            return false;
        }
    }
}