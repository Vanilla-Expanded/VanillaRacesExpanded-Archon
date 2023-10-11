using HarmonyLib;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREArchon
{
    public class VREArchonMod : Mod
    {
        public VREArchonMod(ModContentPack pack) : base(pack)
        {
			new Harmony("VREArchonMod").PatchAll();
        }
    }
}
