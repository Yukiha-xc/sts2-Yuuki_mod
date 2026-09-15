using System;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace yuuki.Scripts.Patches;

[HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
public static class YukiSelectPatch
{
    public static void Prefix(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
    {
        if (characterModel is YukiCharacter)
        {
            try
            {
                string sfxPath = characterModel.CharacterSelectSfx;
                if (!string.IsNullOrEmpty(sfxPath) && sfxPath.StartsWith("res://"))
                {
                    AudioStream val = ResourceLoader.Load<AudioStream>(sfxPath);
                    if (val != null)
                    {
                        AudioStreamPlayer player = new AudioStreamPlayer();
                        player.Stream = val;
                        player.VolumeDb = 0f; // Default volume
                        player.Autoplay = true; // Use Autoplay to ensure it plays right after entering the tree
                        
                        MainLoop mainLoop = Engine.GetMainLoop();
                        MainLoop obj = ((mainLoop is SceneTree) ? mainLoop : null);
                        if (obj != null)
                        {
                            ((Node)((SceneTree)obj).Root).AddChild((Node)(object)player, false, (Godot.Node.InternalMode)0);
                        }
                        
                        player.Finished += delegate
                        {
                            ((Node)player).QueueFree();
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr("[YukiMod] Character Select Audio Error: " + ex.Message);
            }
        }
    }
}
