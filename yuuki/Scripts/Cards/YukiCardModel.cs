using System;
using System.Collections.Generic;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace yuuki.Scripts.Cards;

public abstract class YukiCardModel : CustomCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/" + this.GetType().Name + ".png";

	public virtual int CapacityOverload => 0;

	public virtual bool UsesSnowCrystals => false;

	public virtual bool UsesEmpathy => false;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			if (CapacityOverload > 0)
			{
				yield return (IHoverTip)(object)new HoverTip(new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"), new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"), (Texture2D)null);
				yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>(false);
			}
			if (UsesSnowCrystals)
			{
				yield return (IHoverTip)(object)new HoverTip(new LocString("static_hover_tips", "YUUKI_SNOW_CRYSTAL.title"), new LocString("static_hover_tips", "YUUKI_SNOW_CRYSTAL.description"), (Texture2D)null);
			}
			if (UsesEmpathy)
			{
				yield return (IHoverTip)(object)new HoverTip(new LocString("static_hover_tips", "YUUKI_EMPATHY.title"), new LocString("static_hover_tips", "YUUKI_EMPATHY.description"), (Texture2D)null);
			}
			if (this.GainsBlock)
			{
				yield return HoverTipFactory.Static((StaticHoverTip)5, Array.Empty<DynamicVar>());
			}
		}
	}

	public YukiCardModel(int energyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary)
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary, true)
	{
	}

}

