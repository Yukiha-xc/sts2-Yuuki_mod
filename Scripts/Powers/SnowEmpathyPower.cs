using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;

using
BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class SnowEmpathyPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
public override PowerStackType StackType => PowerStackType.Counter;

    
public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
if (player ==
base.Owner.Player)
        {
            
var aliveEnemies =
base.CombatState.Enemies.Where(e => e.IsAlive).ToList();
var noEmpathyEnemies = aliveEnemies.Where(e => !e.HasPower<EmpathyPower>()).ToList();

            
if (noEmpathyEnemies.Count == 0)
            {
return;
            }

            
if (YukiCrystalSystem.CurrentCrystals >= 2)
            {
                
YukiCrystalSystem.AddCrystals(-2);
                Flash();

                
                System.Random random = new System.Random();
var randomTarget = noEmpathyEnemies[random.Next(noEmpathyEnemies.Count)];

                
await PowerCmd.Apply<EmpathyPower>(randomTarget, 1m,
base.Owner, (CardModel?)null);
            }
        }
    }
public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowEmpathyPower.png";
public override string CustomBigIconPath => "res://yuuki/images/powers/SnowEmpathyPower.png";
}


