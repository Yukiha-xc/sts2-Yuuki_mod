using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using System;

namespace yuuki.Scripts;

public partial class YukiVisuals : NCreatureVisuals
{
    public Sprite2D? Sprite;

    public override void _Ready()
    {
        base._Ready();
        
        
        Sprite = GetNodeOrNull<Sprite2D>("%Visuals") ?? GetNodeOrNull<Sprite2D>("Visuals");
        
        if (Sprite == null)
        {
            foreach (var child in GetChildren())
            {
                if (child is Sprite2D s)
                {
                    Sprite = s;
                    break;
                }
            }
        }
    }

    
    public void PlayAttackDash()
    {
        if (Sprite == null) return;
        var tween = CreateTween();
        tween.TweenProperty(Sprite, "position:x", 40, 0.1);
        tween.TweenProperty(Sprite, "position:x", 0, 0.1);
    }
}
