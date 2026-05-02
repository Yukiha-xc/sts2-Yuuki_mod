using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class CatchSnowball : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/CatchSnowball.png";

    public override bool GainsBlock => true;

    public override bool UsesSnowCrystals => true;

    public CatchSnowball() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8m, ValueProp.Move),
        new IntVar("Turns", 2m),
        new IntVar("NextBlock", 7m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);

        decimal nextBlockAmount = DynamicVars["NextBlock"].BaseValue;
        int turns = (int)base.DynamicVars["Turns"].BaseValue;
        
        var power = await PowerCmd.Apply<CatchSnowballPower>(base.Owner.Creature, (decimal)turns, base.Owner.Creature, this);
        if (power != null)
        {
            power.SetBlock(nextBlockAmount);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["NextBlock"].UpgradeValueBy(3m);
    }
}
