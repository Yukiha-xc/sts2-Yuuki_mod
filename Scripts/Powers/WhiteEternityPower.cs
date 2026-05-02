using BaseLib.Abstracts;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace yuuki.Scripts.Powers;
public sealed class WhiteEternityPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
public override PowerStackType StackType => PowerStackType.Single;
private int _crystalsBefore;

    
public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
if (cardPlay.Card.Owner.Creature ==
base.Owner)
        {
            _crystalsBefore =
YukiCrystalSystem.CurrentCrystals;
        }
return Task.CompletedTask;
    }

    
public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
if (cardPlay.Card.Owner.Creature ==
base.Owner)
        {
            int crystalsAfter =
YukiCrystalSystem.CurrentCrystals;
            
            
if (crystalsAfter < _crystalsBefore)
            {
this.Flash();
                
                
YukiCrystalSystem.AddCrystals(1);
                
                
var player =
base.Owner.Player;
if (player != null)
                {
await CardPileCmd.Draw(choiceContext, 1, player);
                }
            }
        }
    }
public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        
if (side ==
base.Owner.Side)
        {
await PowerCmd.Remove(this);
        }
    }
public override string CustomPackedIconPath => "res://yuuki/images/powers/WhiteEternityPower.png";
public override string CustomBigIconPath => "res://yuuki/images/powers/WhiteEternityPower.png";
}



