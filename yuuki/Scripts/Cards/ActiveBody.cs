using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ActiveBody : YukiCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new DynamicVar("Strength", 1m),
		new DynamicVar("TempStrength", 2m)
	});

	public ActiveBody()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.Owner.Creature.GetPowerAmount<StrengthPower>() > 0)
		{
			await PowerCmd.Apply<ActiveBodyPower>(choiceContext, this.Owner.Creature, this.DynamicVars["TempStrength"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		}
		await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Strength"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Strength"].UpgradeValueBy(1m);
		this.DynamicVars["TempStrength"].UpgradeValueBy(1m);
	}
}
