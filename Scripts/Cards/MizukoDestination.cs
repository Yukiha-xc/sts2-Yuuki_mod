using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MizukoDestination : YukiCardModel
{
    public MizukoDestination() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    public override int CapacityOverload => 1;
    public override bool GainsBlock => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new MizukoBlockVar(),
        new DynamicVar("Power", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        int totalIntentDamage = 0;
        foreach (Creature enemyCreature in base.CombatState.Enemies)
        {
            if (enemyCreature == null || enemyCreature.IsDead || enemyCreature.Monster == null || enemyCreature.Monster.NextMove == null) continue;

            MonsterModel monster = enemyCreature.Monster;
            foreach (AttackIntent intent in monster.NextMove.Intents.OfType<AttackIntent>())
            {
                totalIntentDamage += intent.GetTotalDamage(base.CombatState.PlayerCreatures.Cast<Creature>(), enemyCreature);
            }
        }
        decimal finalBlockValue = Math.Floor((decimal)totalIntentDamage / 2m);
        base.DynamicVars.Block.BaseValue = finalBlockValue;

        
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);

        
        decimal blurAmount = base.DynamicVars["Power"].BaseValue;
        await PowerCmd.Apply<BlurPower>(choiceContext, base.Owner.Creature, blurAmount, base.Owner.Creature, this);
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Power"].UpgradeValueBy(1m);
    }

    public class MizukoBlockVar : BlockVar
    {
        public MizukoBlockVar() : base(0, ValueProp.Move) { }

        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
        {
            if (card == null || card.CombatState == null) return;

            int totalIntentDamage = 0;
            var combatState = card.CombatState;
            if (combatState.Enemies == null) return;

            IEnumerable<Creature> playerCreatures = combatState.PlayerCreatures.Cast<Creature>();

            foreach (Creature enemyCreature in combatState.Enemies)
            {
                if (enemyCreature == null || enemyCreature.IsDead || enemyCreature.Monster == null || enemyCreature.Monster.NextMove == null) continue;

                MonsterModel monster = enemyCreature.Monster;
                foreach (AttackIntent intent in monster.NextMove.Intents.OfType<AttackIntent>())
                {
                    totalIntentDamage += intent.GetTotalDamage(playerCreatures, enemyCreature);
                }
            }

            decimal finalBlock = Math.Floor((decimal)totalIntentDamage / 2m);
            this.BaseValue = finalBlock;
            base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
        }
    }
}

