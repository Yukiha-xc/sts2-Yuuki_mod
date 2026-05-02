using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class AccumulatedSnow : YukiCardModel
{
    public AccumulatedSnow() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true) { }

    public override bool UsesEmpathy => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3m, ValueProp.Move),
        new IntVar("ExtraDamage", 8m),
        new YukiCrystalVar(1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target == null) return;

        bool hasEmpathy = target.HasPower<yuuki.Scripts.Powers.EmpathyPower>();
        decimal damageToDeal = base.DynamicVars.Damage.BaseValue;

        if (hasEmpathy)
        {
            
            await PowerCmd.Remove<yuuki.Scripts.Powers.EmpathyPower>(target);
            
            
            damageToDeal += base.DynamicVars["ExtraDamage"].BaseValue;
            
            
            YukiCrystalSystem.AddCrystals(1);
        }

        
        await DamageCmd.Attack(damageToDeal)
            .FromCard(this)
            .Targeting(target)
            .Execute(choiceContext);
            
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["ExtraDamage"].UpgradeValueBy(3m);
    }
}


