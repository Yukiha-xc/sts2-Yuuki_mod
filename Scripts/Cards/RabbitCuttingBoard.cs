using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class RabbitCuttingBoard : YukiCardModel
{
    private int _reduction = 0;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6m, ValueProp.Move),
        new IntVar("Decrease", 3m)
    ];

    public RabbitCuttingBoard() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);

        _reduction += base.DynamicVars["Decrease"].IntValue;
        UpdateBlock();

        await Cmd.Wait(0.25f);
    }

    private void UpdateBlock()
    {
        decimal baseBlock = IsUpgraded ? 9m : 6m;
        base.DynamicVars.Block.BaseValue = Math.Max(0, baseBlock - _reduction);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
        UpdateBlock();
    }
}
