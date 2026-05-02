using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;
using Godot;

namespace yuuki.Scripts.Patches;


[HarmonyPatch]
public static class AncientDialoguePatches
{
    private const string CharacterId = "YUUKI-YUKI_CHARACTER";

    
    private static void AddYukiDialogues(ref AncientDialogueSet __result, string npcName, int visitCount, string sfx = "")
    {
        if (__result.CharacterDialogues.ContainsKey(CharacterId)) return;

        var dialogues = new List<AncientDialogue>();
        for (int i = 0; i < visitCount; i++)
        {
            
            dialogues.Add(new AncientDialogue($"{npcName}.talk.{CharacterId}.{i}-0.ancient", sfx) 
            { 
                VisitIndex = i 
            });
            
            
            dialogues.Add(new AncientDialogue($"{npcName}.talk.{CharacterId}.{i}-1.char", "") 
            { 
                VisitIndex = i,
                IsRepeating = (i > 0)
            });
        }

        __result.CharacterDialogues[CharacterId] = dialogues;
    }

    [HarmonyPatch(typeof(Neow), "DefineDialogues")]
    [HarmonyPostfix]
    public static void NeowPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "NEOW", 1, "event:/sfx/npcs/neow/neow_welcome");

    [HarmonyPatch(typeof(Tanx), "DefineDialogues")]
    [HarmonyPostfix]
    public static void TanxPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "TANX", 3);

    [HarmonyPatch(typeof(Pael), "DefineDialogues")]
    [HarmonyPostfix]
    public static void PaelPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "PAEL", 2);

    [HarmonyPatch(typeof(Tezcatara), "DefineDialogues")]
    [HarmonyPostfix]
    public static void TezcataraPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "TEZCATARA", 3);

    [HarmonyPatch(typeof(Vakuu), "DefineDialogues")]
    [HarmonyPostfix]
    public static void VakuuPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "VAKUU", 3);

    [HarmonyPatch(typeof(Nonupeipe), "DefineDialogues")]
    [HarmonyPostfix]
    public static void NonupeipePostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "NONUPEIPE", 3, "event:/sfx/npcs/nonupeipe/nonupeipe_welcome");

    [HarmonyPatch(typeof(Orobas), "DefineDialogues")]
    [HarmonyPostfix]
    public static void OrobasPostfix(ref AncientDialogueSet __result) 
        => AddYukiDialogues(ref __result, "OROBAS", 1);

    
    [HarmonyPatch(typeof(TheArchitect), "DefineDialogues")]
    [HarmonyPostfix]
    public static void ArchitectPostfix(ref AncientDialogueSet __result)
    {
        if (__result.CharacterDialogues.ContainsKey(CharacterId)) return;

        var dialogues = new List<AncientDialogue>();
        for (int i = 0; i < 3; i++)
        {
            
            dialogues.Add(new AncientDialogue($"THE_ARCHITECT.talk.{CharacterId}.{i}-0r.ancient", "") { VisitIndex = i, EndAttackers = ArchitectAttackers.Both });
            dialogues.Add(new AncientDialogue($"THE_ARCHITECT.talk.{CharacterId}.{i}-1r.char", "") { VisitIndex = i, EndAttackers = ArchitectAttackers.Both });
            dialogues.Add(new AncientDialogue($"THE_ARCHITECT.talk.{CharacterId}.{i}-2r.ancient", "") { VisitIndex = i, EndAttackers = ArchitectAttackers.Both });
            dialogues.Add(new AncientDialogue($"THE_ARCHITECT.talk.{CharacterId}.{i}-3r.char", "") { VisitIndex = i, EndAttackers = ArchitectAttackers.Both });
            dialogues.Add(new AncientDialogue($"THE_ARCHITECT.talk.{CharacterId}.{i}-4r.ancient", "") { VisitIndex = i, EndAttackers = ArchitectAttackers.Both });
        }
        __result.CharacterDialogues[CharacterId] = dialogues;
    }
}
