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
using MegaCrit.Sts2.Core.HoverTips;
using yuuki.Scripts.Powers;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class GatherSnowball : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/GatherSnowball.png";

    public override bool UsesSnowCrystals => true;

    public GatherSnowball() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<GatherSnowballPower>("MagicNumber", 1m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat(new IHoverTip[] {
        HoverTipFactory.FromPower<GatherSnowballPower>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        
        YukiCrystalSystem.AddCrystals(1);

        int crystalAmount = (int)DynamicVars["MagicNumber"].BaseValue;
        await PowerCmd.Apply<GatherSnowballPower>(base.Owner.Creature, (decimal)crystalAmount, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MagicNumber"].UpgradeValueBy(1m);
    }
}
