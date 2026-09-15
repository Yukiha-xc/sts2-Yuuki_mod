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
public class PsychicGuardian : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Draw", 1m)];

	public PsychicGuardian()
		: base(2, (CardType)3, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<PsychicGuardianPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Draw"].BaseValue, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Draw"].UpgradeValueBy(1m);
	}
}
