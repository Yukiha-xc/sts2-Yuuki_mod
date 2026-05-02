using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

using
BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class DiaryPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
    
    
public override PowerStackType StackType => PowerStackType.Counter;
public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        
if (player ==
base.Owner.Player)
        {
            Flash();

            
var enemies =
base.CombatState.HittableEnemies.Cast<Creature>().ToList();
if (enemies.Count > 0)
            {
await CreatureCmd.Damage(
                    choiceContext, 
                    enemies, 
                    (decimal)base.Amount, 
                    ValueProp.Move,
base.Owner, 
                    null
                );
            }
            
            
await PowerCmd.Remove(this);
        }
    }
}

