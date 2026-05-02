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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Prank : YukiCardModel
{
    
    public Prank() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Weak", 1m),
        new DynamicVar("StrengthLoss", 3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        decimal weakAmount = base.DynamicVars["Weak"].BaseValue;
        decimal strengthLoss = base.DynamicVars["StrengthLoss"].BaseValue;

        
        await PowerCmd.Apply<WeakPower>(base.Owner.Creature, weakAmount, base.Owner.Creature, this);
        await PowerCmd.Apply<PrankPower>(base.Owner.Creature, strengthLoss, base.Owner.Creature, this);

        
        foreach (Creature enemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(enemy, weakAmount, base.Owner.Creature, this);
            await PowerCmd.Apply<PrankPower>(enemy, strengthLoss, base.Owner.Creature, this);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["StrengthLoss"].UpgradeValueBy(4m);
    }
}
