using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PakuPaku : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/PakuPaku.png";

    public PakuPaku() : base(0, CardType.Skill, CardRarity.Common, TargetType.None, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("YukiConsume", 1m),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int consumeAmount = (int)base.DynamicVars["YukiConsume"].BaseValue;
        if (YukiCrystalSystem.CurrentCrystals >= consumeAmount)
        {
            YukiCrystalSystem.AddCrystals(-consumeAmount);
            
            await PlayerCmd.GainEnergy(1, base.Owner);
            
            await CardPileCmd.Draw(choiceContext, base.DynamicVars["Cards"].BaseValue, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Cards"].UpgradeValueBy(1m);
    }
}
