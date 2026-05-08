using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmpathyOfCourage : YukiCardModel
{
    
    public EmpathyOfCourage() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    public override bool UsesEmpathy => true;

    
    public override int CapacityOverload => (int)base.DynamicVars["CapacityOverload"].BaseValue;

    public override string PortraitPath => "res://yuuki/images/cards/RINNE_e03a.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("CapacityOverload", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        int empathyCount = base.CombatState.Enemies.Count(e => e.IsAlive && e.HasPower<EmpathyPower>());
        
        
        if (empathyCount > 0)
        {
            await PlayerCmd.GainEnergy(empathyCount * 2, base.Owner);
        }

        
        int overloadCount = CapacityOverload;
        for (int i = 0; i < overloadCount; i++)
        {
            CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
    }
}


