using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
namespace VREArchon
{

    public class LifeStageWorkSettingsExtension : DefModExtension
    {
        public List<LifeStageWorkSettings> lifeStageWorkSettings = new List<LifeStageWorkSettings>();
    }

    public static class LifeStageWorkSettingsPatches
    {
        public static FieldInfo minAgeField = AccessTools.Field(typeof(LifeStageWorkSettings), "minAge");
        public static MethodInfo overrideMinAgeIfNeededMethod = AccessTools.Method(typeof(LifeStageWorkSettingsPatches), "OverrideMinAgeIfNeeded");
        
        public static int OverrideMinAgeIfNeeded(int minAge, Pawn pawn, LifeStageWorkSettings lifeStageWorkSettings)
        {
            if (pawn.genes != null)
            {
                foreach (var gene in pawn.genes.GenesListForReading)
                {
                    var extension = gene.def.GetModExtension<LifeStageWorkSettingsExtension>();
                    if (extension != null)
                    {
                        var entry = extension.lifeStageWorkSettings.Find(x => x.workType == lifeStageWorkSettings.workType);
                        if (entry != null)
                        {
                            return entry.minAge;
                        }
                    }
                }
            }
            return minAge;
        }
    }


    [HarmonyPatch(typeof(LifeStageWorkSettings), "IsDisabled")]
    public static class LifeStageWorkSettings_IsDisabled_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            foreach (var codeInstruction in codeInstructions)
            {
                yield return codeInstruction;
                if (codeInstruction.LoadsField(LifeStageWorkSettingsPatches.minAgeField))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, LifeStageWorkSettingsPatches.overrideMinAgeIfNeededMethod);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Pawn), "SpecialDisplayStats")]
    public static class Pawn_SpecialDisplayStats_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, Pawn __instance)
        {
            var oldLifestageWorkSettings = __instance.RaceProps.lifeStageWorkSettings;
            if (__instance.genes != null)
            {
                foreach (var gene in __instance.genes.GenesListForReading)
                {
                    var extension = gene.def.GetModExtension<LifeStageWorkSettingsExtension>();
                    if (extension != null)
                    {
                        __instance.RaceProps.lifeStageWorkSettings = extension.lifeStageWorkSettings;
                    }
                }
            }

            foreach (var entry in __result)
            {
                yield return entry;
            }

            __instance.RaceProps.lifeStageWorkSettings = oldLifestageWorkSettings;
        }
    }

    [HarmonyPatch(typeof(Pawn_AgeTracker), "BirthdayBiological")]
    public static class Pawn_AgeTracker_BirthdayBiological_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var pawnField = AccessTools.Field(typeof(Pawn_AgeTracker), "pawn");
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var codeInstruction = codes[i];
                yield return codeInstruction;
                if (codeInstruction.LoadsField(LifeStageWorkSettingsPatches.minAgeField))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnField);
                    yield return codes[i - 3];
                    yield return codes[i - 2];
                    yield return codes[i - 1];
                    yield return new CodeInstruction(OpCodes.Call, LifeStageWorkSettingsPatches.overrideMinAgeIfNeededMethod);
                }
            }
        }
    }

    [HarmonyPatch(typeof(LifeStageWorker_HumanlikeChild), "Notify_LifeStageStarted")]
    public static class LifeStageWorker_HumanlikeChild_Notify_LifeStageStarted_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var codeInstruction = codes[i];
                yield return codeInstruction;
                if (codeInstruction.LoadsField(LifeStageWorkSettingsPatches.minAgeField))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return codes[i - 3];
                    yield return codes[i - 2];
                    yield return codes[i - 1];
                    yield return new CodeInstruction(OpCodes.Call, LifeStageWorkSettingsPatches.overrideMinAgeIfNeededMethod);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PawnUtility), "IsWorkTypeDisabledByAge")]
    public static class PawnUtility_IsWorkTypeDisabledByAge_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var codeInstruction = codes[i];
                yield return codeInstruction;
                if (codeInstruction.LoadsField(LifeStageWorkSettingsPatches.minAgeField))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Call, LifeStageWorkSettingsPatches.overrideMinAgeIfNeededMethod);
                }
            }
        }
    }
}
