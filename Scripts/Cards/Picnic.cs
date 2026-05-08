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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Picnic : YukiCardModel
{
    public Picnic() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true) { }
    
    public override string PortraitPath => "res://yuuki/images/cards/YUUKI_FD_e02a.png";

    public override bool UsesEmpathy => true;
    public override int CapacityOverload => 1;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(15m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var aliveEnemies = base.CombatState.HittableEnemies.ToList();
        bool allHaveEmpathy = aliveEnemies.All(e => e.HasPower<EmpathyPower>());

        if (allHaveEmpathy && aliveEnemies.Count > 0)
        {
            int enemyCount = aliveEnemies.Count;
            
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .WithHitCount(enemyCount)
                .TargetingAllOpponents(base.CombatState)
                .Execute(choiceContext);
        }
        else
        {
            
            foreach (var enemy in aliveEnemies)
            {
                await PowerCmd.Apply<EmpathyPower>(choiceContext, enemy, 1m, base.Owner.Creature, this);
            }

            CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}



