using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ActingCoquettishly : YukiCardModel
{
public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

	public ActingCoquettishly()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		List<CardModel> list = PileTypeExtensions.GetPile(PileType.Hand, this.Owner).Cards.Where((CardModel c) => c != null && (object)c != this).ToList();
		if (list.Count > 0)
		{
			CardModel val = list[this.Owner.RunState.Rng.Shuffle.NextInt(0, list.Count)];
			Creature? val2 = null;
			if ((int)val.TargetType == 2)
			{
				List<Creature> list2 = combatState.Enemies.Where((Creature e) => e.IsAlive).ToList();
				if (list2.Count > 0)
				{
					val2 = list2[this.Owner.RunState.Rng.Shuffle.NextInt(0, list2.Count)];
				}
			}
			await CardCmd.AutoPlay(choiceContext, val, val2, AutoPlayType.Default, false, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}
