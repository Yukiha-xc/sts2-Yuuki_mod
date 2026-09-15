using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class DesiringFootsteps : YukiCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new BlockVar(10m, ValueProp.Move),
		new DamageVar(7m, ValueProp.Move)
	});

	public override int CapacityOverload => 1;

	public DesiringFootsteps()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if ((object)card == this && this.CombatState != null && this.CombatState.Enemies != null)
		{
			List<Creature> list = this.CombatState.Enemies.Where((Creature e) => e != null && e.IsAlive).ToList();
			if (list.Count > 0)
			{
				int index = this.Owner.RunState.Rng.Shuffle.NextInt(0, list.Count);
				Creature val = list[index];
				await CreatureCmd.Damage(choiceContext, val, this.DynamicVars["Damage"].BaseValue, ValueProp.Move, this.Owner.Creature, this, null);
			}
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars["Block"].BaseValue, ValueProp.Move, cardPlay, false);
		await CardPileCmd.Add(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, CardPilePosition.Bottom, null, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Block"].UpgradeValueBy(4m);
		this.DynamicVars["Damage"].UpgradeValueBy(3m);
	}
}

