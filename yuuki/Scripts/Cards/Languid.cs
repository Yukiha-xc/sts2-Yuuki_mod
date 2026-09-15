using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Languid : YukiCardModel
{
	public override bool GainsBlock => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[3]
	{
		new DynamicVar("Block", 12m),
		new DynamicVar("Cards", 1m),
		new DynamicVar("Energy", 1m)
	});

	public Languid()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars["Block"].BaseValue, ValueProp.Move, cardPlay, false);
		await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Cards"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Energy"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		PlayerCmd.EndTurn(this.Owner, false, null);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Block"].UpgradeValueBy(4m);
	}
}
