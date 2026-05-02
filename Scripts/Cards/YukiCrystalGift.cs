using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class YukiCrystalGift : YukiCardModel
{
    public YukiCrystalGift() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e005a.png";

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new YukiCrystalVar(1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPile drawPile = PileType.Draw.GetPile(base.Owner);
        await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner);
        CardModel topCard = drawPile.Cards.FirstOrDefault();
        if (topCard != null)
        {
            await CardCmd.Exhaust(choiceContext, topCard);
        }

        await PlayerCmd.GainEnergy(1, base.Owner);

        int crystalAmount = (int)base.DynamicVars[YukiCrystalVar.Key].BaseValue;
        YukiCrystalSystem.AddCrystals(crystalAmount);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars[YukiCrystalVar.Key].UpgradeValueBy(1m);
    }
}
