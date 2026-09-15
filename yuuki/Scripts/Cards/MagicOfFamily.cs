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
public class MagicOfFamily : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Heal", 4m)];

public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

	public MagicOfFamily()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars["Heal"].BaseValue, true);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Heal"].UpgradeValueBy(2m);
	}
}
