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
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Combat;

using MegaCrit.Sts2.Core.ValueProps;
namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SeveranceOfConfusion : YukiCardModel
{
    public SeveranceOfConfusion() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move),
        new DynamicVar("Power", 1m)
    ];

    public override int CapacityOverload => 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var enemy in base.CombatState.Enemies)
        {
            if (enemy.IsAlive)
            {
                if (enemy.Block > 0)
                {
                    await CreatureCmd.LoseBlock(enemy, enemy.Block);
                }
                await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this);
            }
        }

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);

        CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}



