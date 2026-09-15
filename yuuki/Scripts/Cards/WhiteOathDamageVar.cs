using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

public class WhiteOathDamageVar : DamageVar
{
	public WhiteOathDamageVar()
		: base(0m, (ValueProp)8)
	{
	}

	public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
	{
		int num = ((card.CombatState != null) ? YukiCrystalSystem.CurrentCrystals : 0);
		decimal baseValue = 4m;
		if (card.DynamicVars.TryGetValue("Multiplier", out var mVar))
		{
			baseValue = mVar.BaseValue;
		}
		this.BaseValue = (decimal)num * baseValue;
		base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
	}
}
