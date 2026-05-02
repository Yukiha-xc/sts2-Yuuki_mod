using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class NoMoreGoodbyesPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner.Creature == base.Owner && cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust))
        {
            Flash();
            
            Player? player = base.Owner.Player;
            if (player != null && player.Creature.CombatState != null)
            {
                
                CardModel copy = player.Creature.CombatState.CreateCard(cardPlay.Card.CanonicalInstance, player);
                
                
                CardCmd.ApplyKeyword(copy, CardKeyword.Ethereal);
                
                
                
                await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, true);
            }
            
            
            await PowerCmd.Decrement(this);
        }
    }

    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side)
        {
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/NoMoreGoodbyesPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/NoMoreGoodbyesPower.png";
}
