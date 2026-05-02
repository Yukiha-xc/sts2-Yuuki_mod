using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCook : YukiCardModel
{
    public SnowCook() : base(1, CardType.Skill, CardRarity.Common, TargetType.None, true) { }

    public override bool GainsBlock => true;
    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8m, ValueProp.Move),
        new YukiCrystalVar(1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await MegaCrit.Sts2.Core.Commands.CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
        YukiCrystalSystem.AddCrystals(1);
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}
