using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes;
using Godot;
using System;

namespace yuuki.Scripts.Patches;




[HarmonyPatch(typeof(SfxCmd), nameof(SfxCmd.Play), typeof(string), typeof(float))]
public static class AudioPatches
{
    public static bool Prefix(string sfx, float volume)
    {
        
        if (!string.IsNullOrEmpty(sfx) && sfx.StartsWith("res://"))
        {
            try
            {
                var stream = ResourceLoader.Load<AudioStream>(sfx);
                if (stream != null)
                {
                    var player = new AudioStreamPlayer();
                    player.Stream = stream;
                    player.VolumeDb = Mathf.LinearToDb(volume);
                    player.Bus = "Sfx";

                    
                    var root = Engine.GetMainLoop() as SceneTree;
                    root?.Root.AddChild(player);
                    
                    player.Play();
                    player.Finished += () => player.QueueFree();
                    
                    return false; 
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[YukiMod] Audio Error: {ex.Message}");
                return true;
            }
        }
        return true;
    }
}
