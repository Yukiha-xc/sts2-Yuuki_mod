using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialBlackCat : YukiCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>((CardKeyword[])(object)new CardKeyword[2]
	{
		(CardKeyword)2,
		(CardKeyword)1
	});

	public override string PortraitPath => "res://yuuki/images/cards/BlackCat.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new IntVar("Reduction", 6m)];

	public SpecialBlackCat()
		: base(1, (CardType)2, (CardRarity)7, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		decimal reduction = this.DynamicVars["Reduction"].BaseValue;
		BlackCatPower blackCatPower = await PowerCmd.Apply<BlackCatPower>(choiceContext, this.Owner.Creature, 2m, this.Owner.Creature, (CardModel)this, false);
		if (blackCatPower != null)
		{
			blackCatPower.ReductionAmount = reduction;
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Reduction"].UpgradeValueBy(3m);
	}
}
