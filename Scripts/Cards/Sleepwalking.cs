using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Sleepwalking : YukiCardModel
{
    public override bool GainsBlock => true;

    public Sleepwalking() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(7m, ValueProp.Move),
        new DynamicVar("DexterityLoss", 1m),
        new DynamicVar("EnergyNextTurn", 2m)
    ];

    public override string PortraitPath => "res://yuuki/images/cards/OCHIBA_e03a.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);

        
        await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, -1m, base.Owner.Creature, this);

        
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, 2m, base.Owner.Creature, this);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}

