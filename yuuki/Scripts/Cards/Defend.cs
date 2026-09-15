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
public class Defend : YukiCardModel
{
	private const int energyCost = 1;

	private const CardType type = CardType.Skill;

	private const CardRarity rarity = CardRarity.Basic;

	private const TargetType targetType = TargetType.None;

	private const bool shouldShowInCardLibrary = true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new BlockVar(5m, ValueProp.Move)];

	public override bool GainsBlock => true;

	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };

	public Defend()
		: base(1, CardType.Skill, CardRarity.Basic, TargetType.None, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
	}
}
