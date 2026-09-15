using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace yuuki.Scripts;

[ScriptPath("res://Scripts/YukiVisuals.cs")]
public partial class YukiVisuals : NCreatureVisuals
{
	public Sprite2D? Sprite;

	public override void _Ready()
	{
		base._Ready();
		Sprite = this.GetNodeOrNull<Sprite2D>(new NodePath("%Visuals")) ?? this.GetNodeOrNull<Sprite2D>(new NodePath("Visuals"));
		if (Sprite != null)
		{
			return;
		}
		foreach (Node child in this.GetChildren(false))
		{
			Sprite2D val = (Sprite2D)(object)((child is Sprite2D) ? child : null);
			if (val != null)
			{
				Sprite = val;
				break;
			}
		}
	}
}
