using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class FallingSnow : YukiCardModel
{
	private const int energyCost = 1;

	private const CardType type = CardType.Skill;

	private const CardRarity rarity = CardRarity.Basic;

	private const TargetType targetType = TargetType.None;

	private const bool shouldShowInCardLibrary = true;

	public override bool UsesSnowCrystals => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new YukiCrystalVar(2m)];

	public FallingSnow()
		: base(1, CardType.Skill, CardRarity.Basic, TargetType.None, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await YukiCrystalSystem.AddCrystals((int)this.DynamicVars["YukiCrystal"].BaseValue);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiCrystal"].UpgradeValueBy(1m);
	}
}
