using System.Linq;
using Verse;
using VFECore.Abilities;

namespace VREArchon
{
    public static class ModCompatibility
    {
        public static bool VPELoaded = ModsConfig.IsActive("VanillaExpanded.VPsycastsE");

        public static AbilityDef RandomPsycastDef()
        {
            return DefDatabase<AbilityDef>.AllDefs.Where(x => VanillaPsycastsExpanded.AbilityExtensionPsycastUtility
            .Psycast(x) != null).RandomElement();
        }

        public static bool PawnIsPsycaster(Pawn pawn)
        {
            return VanillaPsycastsExpanded.PsycastUtility.Psycasts(pawn) != null;
        }
    }
}
