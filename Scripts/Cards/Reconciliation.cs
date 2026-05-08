using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Reconciliation : YukiCardModel
{
    
    public Reconciliation() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies, true) 
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Power", 3m)
    ];

    public override bool UsesEmpathy => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int debuffAmount = (int)base.DynamicVars["Power"].BaseValue;
        bool anyRemoved = false;

        
        foreach (Creature enemy in base.CombatState.HittableEnemies)
        {
            if (enemy.HasPower<EmpathyPower>())
            {
                anyRemoved = true;
                
                await PowerCmd.Remove<EmpathyPower>(enemy);
                
                if (enemy.IsAlive)
                {
                    await PowerCmd.Apply<WeakPower>(choiceContext, enemy, (decimal)debuffAmount, base.Owner.Creature, this);
                    await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, (decimal)debuffAmount, base.Owner.Creature, this);
                }
            }
        }

        

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Power"].UpgradeValueBy(2m);
    }
}

