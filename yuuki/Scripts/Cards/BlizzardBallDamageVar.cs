using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

public class BlizzardBallDamageVar : DamageVar
{
	public BlizzardBallDamageVar(decimal baseVal)
		: base(baseVal, ValueProp.Move)
	{
	}

	public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
	{
		int num = ((card.CombatState != null) ? YukiCrystalSystem.CurrentCrystals : 0);
		decimal num2 = (card.IsUpgraded ? 11m : 7m);
		this.BaseValue = num2 + (decimal)num * 2m;
		base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
	}
}
