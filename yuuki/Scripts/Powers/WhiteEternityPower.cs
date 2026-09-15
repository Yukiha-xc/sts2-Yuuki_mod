using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public sealed class WhiteEternityPower : CustomPowerModel
{
	private int _crystalsBefore;

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/WhiteEternityPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/WhiteEternityPower.png";

	public override Task BeforeCardPlayed(CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == this.Owner)
		{
			_crystalsBefore = YukiCrystalSystem.CurrentCrystals;
		}
		return Task.CompletedTask;
	}

	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == this.Owner && YukiCrystalSystem.CurrentCrystals < _crystalsBefore)
		{
			this.Flash();
			int amount = this.Amount;
			YukiCrystalSystem.AddCrystals(amount);
			Player player = this.Owner.Player;
			if (player != null)
			{
				await CardPileCmd.Draw(choiceContext, amount, player);
			}
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == this.Owner.Side)
		{
			await PowerCmd.Remove((PowerModel?)(object)this);
		}
	}
}
