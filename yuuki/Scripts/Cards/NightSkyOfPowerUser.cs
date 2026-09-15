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
public class NightSkyOfPowerUser : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new IntVar("DamageInc", 25m)];

	public NightSkyOfPowerUser()
		: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<NightSkyOfPowerUserPower>(choiceContext, this.Owner.Creature, this.DynamicVars["DamageInc"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["DamageInc"].UpgradeValueBy(25m);
	}
}
