using BaseLib.Abstracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public class InnocentProphecyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            
            
            
            await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner.Player);
            var drawPile = PileType.Draw.GetPile(base.Owner.Player).Cards;
            
            if (drawPile.Any())
            {
                
                var sortedCards = drawPile.OrderBy(c => c.Rarity).ThenBy(c => c.Id).ToList();
                
                CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, (int)base.Amount);
                var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, sortedCards, base.Owner.Player, prefs);
                
                if (selectedCards.Any())
                {
                    await CardPileCmd.Add(selectedCards.First(), PileType.Hand);
                }
            }
            
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/InnocentProphecyPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/InnocentProphecyPower.png";
}
