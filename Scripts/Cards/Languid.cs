using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Languid : YukiCardModel
{
    public override bool GainsBlock => true;

    public Languid() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Block", 12m),
        new DynamicVar("Cards", 1m),
        new DynamicVar("Energy", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars["Block"].BaseValue, ValueProp.Move, cardPlay);

        await PowerCmd.Apply<DrawCardsNextTurnPower>(
            base.Owner.Creature, 
            base.DynamicVars["Cards"].BaseValue, 
            base.Owner.Creature, 
            this
        );

        await PowerCmd.Apply<EnergyNextTurnPower>(
            base.Owner.Creature, 
            base.DynamicVars["Energy"].BaseValue, 
            base.Owner.Creature, 
            this
        );

        PlayerCmd.EndTurn(base.Owner, canBackOut: false);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Block"].UpgradeValueBy(4m);
    }
}
