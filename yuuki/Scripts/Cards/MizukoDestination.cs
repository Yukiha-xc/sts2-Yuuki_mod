using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MizukoDestination : YukiCardModel
{
	public class MizukoBlockVar : BlockVar
	{
		public MizukoBlockVar()
			: base(0m, (ValueProp)8)
		{
		}

		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
		{
			if (card == null || card.CombatState == null)
			{
				return;
			}
			int num = 0;
			ICombatState combatState = card.CombatState;
			if (combatState.Enemies == null)
			{
				return;
			}
			IEnumerable<Creature> enumerable = combatState.PlayerCreatures.Cast<Creature>();
			foreach (Creature enemy in combatState.Enemies)
			{
				if (enemy == null || enemy.IsDead || enemy.Monster == null || enemy.Monster.NextMove == null)
				{
					continue;
				}
				foreach (AttackIntent item in enemy.Monster.NextMove.Intents.OfType<AttackIntent>())
				{
					num += item.GetTotalDamage(enumerable, enemy);
				}
			}
			decimal baseValue = (decimal)num;
			this.BaseValue = baseValue;
			base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
		}
	}

	public override int CapacityOverload => 1;

	public override bool GainsBlock => true;

public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new MizukoBlockVar(),
		new DynamicVar("Power", 1m)
	});

	public MizukoDestination()
		: base(1, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = 0;
		foreach (Creature enemy in this.CombatState.Enemies)
		{
			if (enemy == null || enemy.IsDead || enemy.Monster == null || enemy.Monster.NextMove == null)
			{
				continue;
			}
			foreach (AttackIntent item in enemy.Monster.NextMove.Intents.OfType<AttackIntent>())
			{
				num += item.GetTotalDamage(this.CombatState.PlayerCreatures.Cast<Creature>(), enemy);
			}
		}
		decimal baseValue = (decimal)num;
		((DynamicVar)this.DynamicVars.Block).BaseValue = baseValue;
		await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, (ValueProp)8, cardPlay, false);
		decimal baseValue2 = this.DynamicVars["Power"].BaseValue;
		await PowerCmd.Apply<BlurPower>(choiceContext, this.Owner.Creature, baseValue2, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Power"].UpgradeValueBy(1m);
	}
}

