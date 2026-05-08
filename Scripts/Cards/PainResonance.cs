using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PainResonance : YukiCardModel
{
    public PainResonance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, true) { }

    public override bool UsesEmpathy => true;

    protected override bool ShouldGlowGoldInternal => base.CombatState?.HittableEnemies.Any((Creature e) => e.HasPower<EmpathyPower>()) ?? false;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move),
        new IntVar("WeakAmount", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var empathyEnemies = base.CombatState.Enemies.Where(e => e != null && e.IsAlive && e.HasPower<EmpathyPower>()).ToList();

        if (empathyEnemies.Count > 0)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            foreach (var enemy in empathyEnemies)
            {
                await PowerCmd.Remove<EmpathyPower>(enemy);

                
                if (enemy.IsAlive)
                {
                    await PowerCmd.Apply<WeakPower>(choiceContext, enemy, base.DynamicVars["WeakAmount"].BaseValue, base.Owner.Creature, this);

                    await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                        .WithHitCount(2)
                        .FromCard(this)
                        .Targeting(enemy)
                        .Execute(choiceContext);
                }
            }
        }
        else
        {
            await Cmd.Wait(0.1f);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["WeakAmount"].UpgradeValueBy(1m);
    }
}

