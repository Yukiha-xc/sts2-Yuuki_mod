using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Strike : YukiCardModel
{
	private const int energyCost = 1;

	private const CardType type = CardType.Attack;

	private const CardRarity rarity = CardRarity.Basic;

	private const TargetType targetType = TargetType.AnyEnemy;

	private const bool shouldShowInCardLibrary = true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(6m, ValueProp.Move)];

	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

	public Strike()
		: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target is not { } target)
		{
			return;
		}

		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(target)
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}
