using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class LittleStars : YukiCardModel
{
    
    public LittleStars() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true) { }
    
    public override string PortraitPath => "res://yuuki/images/cards/BG057_005.png";

    public override bool UsesEmpathy => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3m, ValueProp.Move),
        new IntVar("ExtraHits", 3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target == null) return;

        bool hasEmpathy = target.HasPower<yuuki.Scripts.Powers.EmpathyPower>();
        int totalHits = 2;

        if (hasEmpathy)
        {
            
            await PowerCmd.Remove<yuuki.Scripts.Powers.EmpathyPower>(target);
            
            
            totalHits += (int)base.DynamicVars["ExtraHits"].BaseValue;
        }

        
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(totalHits)
            .Targeting(target)
            .Execute(choiceContext);
            
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["ExtraHits"].UpgradeValueBy(1m);
    }
}


