using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Fimbulwinter : YukiCardModel
{
    public Fimbulwinter() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    public override string PortraitPath => "res://yuuki/images/cards/Fimbulwinter_Portrait.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Threshold", 4m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FimbulwinterPower>(base.Owner.Creature, base.DynamicVars["Threshold"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Threshold"].UpgradeValueBy(1m);
    }
}
