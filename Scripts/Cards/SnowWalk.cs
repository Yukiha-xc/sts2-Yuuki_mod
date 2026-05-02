using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowWalk : YukiCardModel
{
    
public override string PortraitPath => "res://yuuki/images/cards/WalkingInTheSnow.png";

    public SnowWalk() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        
        await PowerCmd.Apply<SnowWalkPower>(
            base.Owner.Creature, 
            1m, 
            base.Owner.Creature, 
            this
        );
    }

    protected override void OnUpgrade()
    {
        
        ((CardModel)this).EnergyCost.UpgradeBy(-1);
    }
}

