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

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Mending : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/Mending.png";

    public override bool UsesSnowCrystals => true;

    public Mending() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(4m),
        new DynamicVar("YukiConsume", 1m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat(new IHoverTip[] {
        HoverTipFactory.FromPower<VigorPower>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (YukiCrystalSystem.CurrentCrystals >= 1)
        {
            YukiCrystalSystem.AddCrystals(-1);

            int vigorAmount = (int)DynamicVars["VigorPower"].BaseValue;
            await PowerCmd.Apply<VigorPower>(base.Owner.Creature, (decimal)vigorAmount, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2m);
    }
}
