using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class LonelySnow : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/LonelySnow.png";

    public LonelySnow() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("BlockPerCrystal", 3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        
        await PowerCmd.Apply<LonelySnowPower>(
            base.Owner.Creature, 
            base.DynamicVars["BlockPerCrystal"].BaseValue, 
            base.Owner.Creature, 
            this
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BlockPerCrystal"].UpgradeValueBy(1m);
    }
}
