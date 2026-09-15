using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialMemory : YukiCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>((CardKeyword[])(object)new CardKeyword[2]
	{
		CardKeyword.Retain,
		CardKeyword.Exhaust
	});

	public override string PortraitPath => "res://yuuki/images/cards/memory.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new IntVar("Draw", 4m)];

	public SpecialMemory()
		: base(0, CardType.Skill, CardRarity.Token, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CardPileCmd.Draw(choiceContext, (decimal)(int)this.DynamicVars["Draw"].BaseValue, this.Owner, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Draw"].UpgradeValueBy(2m);
	}
}
