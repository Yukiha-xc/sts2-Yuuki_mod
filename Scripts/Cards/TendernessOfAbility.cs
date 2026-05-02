using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class TendernessOfAbility : YukiCardModel
{
    public TendernessOfAbility() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy, true) { }

    public override string PortraitPath => "res://yuuki/images/cards/YUKI_h01a1.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("Hits").WithMultiplier((CardModel card, Creature? _) => GetVoidCount(card))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int voidCount = GetVoidCount(this);
        
        if (voidCount > 0)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .WithHitCount(voidCount)
                .TargetingRandomOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
        else
        {
            await Cmd.Wait(0.1f);
        }
        
        await Cmd.Wait(0.25f);
    }

    private static int GetVoidCount(CardModel card)
    {
        var exhaustPile = PileType.Exhaust.GetPile(card.Owner);
        if (exhaustPile == null) return 0;
        return exhaustPile.Cards.Count(c => c.Type == CardType.Status);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}

