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
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Graduation : YukiCardModel
{
	public override bool GainsBlock => true;

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(7m, ValueProp.Move),
		(DynamicVar)new BlockVar(2m, ValueProp.Move)
	});

	public Graduation()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.None, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int crystalCount = YukiCrystalSystem.CurrentCrystals;
		if (crystalCount <= 0)
		{
			return;
		}
		ICombatState? combatState = this.Owner.Creature.CombatState;
		if (combatState == null)
		{
			return;
		}
		for (int i = 0; i < crystalCount; i++)
		{
			List<Creature> list = combatState.Enemies.Where((Creature e) => e.IsAlive).ToList();
			if (list.Count == 0)
			{
				break;
			}
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).TargetingRandomOpponents(combatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, ValueProp.Move, cardPlay, false);
		}
		await YukiCrystalSystem.AddCrystals(-crystalCount);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(1m);
	}
}
