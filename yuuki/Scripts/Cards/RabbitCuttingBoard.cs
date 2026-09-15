using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class RabbitCuttingBoard : YukiCardModel
{
	private int _reduction;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new BlockVar(7m, (ValueProp)8),
		(DynamicVar)new IntVar("Decrease", 2m)
	});

	public RabbitCuttingBoard()
		: base(0, (CardType)2, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay, false);
		_reduction += this.DynamicVars["Decrease"].IntValue;
		UpdateBlock();
		await Cmd.Wait(0.25f, false);
	}

	private void UpdateBlock()
	{
		decimal num = (this.IsUpgraded ? 10m : 7m);
		((DynamicVar)this.DynamicVars.Block).BaseValue = Math.Max(0m, num - (decimal)_reduction);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
		UpdateBlock();
	}
}
