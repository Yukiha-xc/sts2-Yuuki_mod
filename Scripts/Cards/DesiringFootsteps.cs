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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class DesiringFootsteps : YukiCardModel
{
    public DesiringFootsteps() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Block", 10m),
        new DynamicVar("Damage", 7m)
    ];

    public override int CapacityOverload => 1;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card == this && base.CombatState != null && base.CombatState.Enemies != null)
        {
            var enemies = base.CombatState.Enemies.Where(e => e != null && e.IsAlive).ToList();
            if (enemies.Count > 0)
            {
                int index = base.Owner.RunState.Rng.Shuffle.NextInt(0, enemies.Count);
                var target = enemies[index];
                
                await CreatureCmd.Damage(
                    choiceContext, 
                    target, 
                    base.DynamicVars["Damage"].BaseValue, 
                    ValueProp.Move, 
                    base.Owner.Creature, 
                    this
                );
            }
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars["Block"].BaseValue, ValueProp.Move, cardPlay);

        CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
        await CardPileCmd.Add(voidCard, PileType.Discard);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Block"].UpgradeValueBy(4m);
        base.DynamicVars["Damage"].UpgradeValueBy(3m);
    }
}
