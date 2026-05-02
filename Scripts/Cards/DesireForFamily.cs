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

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class DesireForFamily : YukiCardModel
{
    public DesireForFamily() : base(0, CardType.Skill, CardRarity.Common, TargetType.None, true) { }

    public override int CapacityOverload => 1;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        this.ExhaustOnNextPlay = true;

        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);

        CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, true));
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
