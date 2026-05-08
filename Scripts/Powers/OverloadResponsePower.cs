using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public sealed class OverloadResponsePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != base.Owner)
        {
            return playCount;
        }

        
        int num = CombatManager.Instance.History.CardPlaysStarted.Count(
            (CardPlayStartedEntry e) => e.Actor == base.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(base.CombatState)
        );

        
        if (num < base.Amount)
        {
            return playCount + 1;
        }
        return playCount;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        
        
        CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(card.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/OverloadResponsePower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/OverloadResponsePower.png";
}

