using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Graduation : YukiCardModel
{
    public Graduation() : base(2, CardType.Attack, CardRarity.Rare, TargetType.None, true) { }

    public override bool GainsBlock => true;

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move), 
        new BlockVar(2m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int crystalCount = YukiCrystalSystem.CurrentCrystals;
        if (crystalCount <= 0) return;

        CombatState? combatState = Owner.Creature.CombatState;
        if (combatState == null) return;

        System.Random random = new System.Random();

        for (int i = 0; i < crystalCount; i++)
        {
            var aliveEnemies = combatState.Enemies
                .Where(e => e.IsAlive)
                .ToList();

            if (aliveEnemies.Count == 0) break;

            var randomTarget = aliveEnemies[random.Next(aliveEnemies.Count)];

            await MegaCrit.Sts2.Core.Commands.DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(randomTarget)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);
        }

        YukiCrystalSystem.AddCrystals(-crystalCount);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m); 
        DynamicVars.Block.UpgradeValueBy(1m);
    }
}
