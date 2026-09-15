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

public sealed class NoMoreGoodbyesPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/NoMoreGoodbyesPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/NoMoreGoodbyesPower.png";

	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == this.Owner && cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust))
		{
			this.Flash();
			Player player = this.Owner.Player;
			if (player != null && player.Creature.CombatState != null)
			{
				CardModel card = player.Creature.CombatState.CreateCard(cardPlay.Card.CanonicalInstance, player);
				CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
				await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, null);
			}
			await PowerCmd.Decrement((PowerModel)(object)this);
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
