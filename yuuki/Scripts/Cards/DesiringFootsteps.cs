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
		new BlockVar(10m, (ValueProp)8),
		new DamageVar(7m, (ValueProp)8)
	});

	public override int CapacityOverload => 1;

	public DesiringFootsteps()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
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
				await CreatureCmd.Damage(choiceContext, val, this.DynamicVars["Damage"].BaseValue, (ValueProp)8, this.Owner.Creature, (CardModel)this);
			}
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars["Block"].BaseValue, (ValueProp)8, cardPlay, false);
		await CardPileCmd.Add(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), (PileType)3, (CardPilePosition)1, (AbstractModel)null, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Block"].UpgradeValueBy(4m);
		this.DynamicVars["Damage"].UpgradeValueBy(3m);
	}
}

