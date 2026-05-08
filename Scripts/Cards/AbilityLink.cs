using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class AbilityLink : YukiCardModel
{
    
    public AbilityLink() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        this.ExhaustOnNextPlay = true;

        decimal totalStr = 0;
        decimal totalDex = 0;

        
        foreach (var enemy in base.CombatState.Enemies)
        {
            
            if (enemy.IsAlive && enemy.HasPower<EmpathyPower>())
            {
                
                var strPower = enemy.GetPower<StrengthPower>();
                if (strPower != null)
                {
                    totalStr += strPower.Amount;
                }

                var dexPower = enemy.GetPower<DexterityPower>();
                if (dexPower != null)
                {
                    totalDex += dexPower.Amount;
                }
            }
        }

        
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, totalStr + 1m, base.Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, totalDex + 1m, base.Owner.Creature, this);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}


