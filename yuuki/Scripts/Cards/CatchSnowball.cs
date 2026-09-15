using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class CatchSnowball : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/CatchSnowball.png";

	public override bool GainsBlock => true;

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[3]
	{
		(DynamicVar)new BlockVar(8m, ValueProp.Move),
		(DynamicVar)new IntVar("Turns", 2m),
		(DynamicVar)new BlockVar("NextBlock", 7m, ValueProp.Move)
	});

	public CatchSnowball()
		: base(2, CardType.Skill, CardRarity.Common, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay, false);
		BlockVar val = (BlockVar)this.DynamicVars["NextBlock"];
		IEnumerable<AbstractModel>? enumerable;
		decimal nextBlockAmount = Hook.ModifyBlock(combatState, this.Owner.Creature, ((DynamicVar)val).BaseValue, val.Props, this, cardPlay, out enumerable);
		int num = (int)this.DynamicVars["Turns"].BaseValue;
		(await PowerCmd.Apply<CatchSnowballPower>(choiceContext, this.Owner.Creature, (decimal)num, this.Owner.Creature, (CardModel)this, false))?.SetBlock(nextBlockAmount);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["NextBlock"].UpgradeValueBy(3m);
	}
}

