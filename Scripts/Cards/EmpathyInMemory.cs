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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmpathyInMemory : YukiCardModel
{
    
    public EmpathyInMemory() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None, true) { }

    public override bool UsesEmpathy => true;

    
public override string PortraitPath => "res://yuuki/images/cards/YUKI_e01a2.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<EmpathyInMemoryPower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}

