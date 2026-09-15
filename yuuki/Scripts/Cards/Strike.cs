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

	private const CardType type = (CardType)1;

	private const CardRarity rarity = (CardRarity)1;

	private const TargetType targetType = (TargetType)2;

	private const bool shouldShowInCardLibrary = true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(6m, (ValueProp)8)];

	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { (CardTag)1 };

	public Strike()
		: base(1, (CardType)1, (CardRarity)1, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}
