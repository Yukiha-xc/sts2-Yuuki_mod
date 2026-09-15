using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace yuuki.Scripts.Powers;

public sealed class OverloadResponsePower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/OverloadResponsePower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/OverloadResponsePower.png";

	public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
	{
		if (card.Owner.Creature != this.Owner)
		{
			return playCount;
		}
		if (CombatManager.Instance.History.CardPlaysStarted.Count((CardPlayStartedEntry e) => e.Actor == this.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(this.CombatState)) < this.Amount)
		{
			return playCount + 1;
		}
		return playCount;
	}

	public override async Task AfterModifyingCardPlayCount(CardModel card)
	{
		this.Flash();
		await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(card.Owner), PileType.Discard, null);
	}
}
