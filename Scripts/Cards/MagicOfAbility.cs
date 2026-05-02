using System;
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
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MagicOfAbility : YukiCardModel
{
    
    public MagicOfAbility() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true) { }

    
    public override int CapacityOverload => 1;

    public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e702a.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Damage", 9m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
 PowerCmd.Apply<MagicOfAbilityPower>(base.Owner.Creature, base.DynamicVars["Damage"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Damage"].UpgradeValueBy(2m);
    }
}

