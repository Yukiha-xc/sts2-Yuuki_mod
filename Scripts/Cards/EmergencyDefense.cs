using System.Collections.Generic;
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
public class EmergencyDefense : YukiCardModel
{
    public EmergencyDefense() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6m, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            foreach (var tip in base.ExtraHoverTips) yield return tip;
            yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Dazed>();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);

        CardModel dazedCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Dazed>(base.Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(dazedCard, PileType.Discard, true));
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}
