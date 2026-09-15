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

	private const CardType type = (CardType)2;

	private const CardRarity rarity = (CardRarity)1;

	private const TargetType targetType = (TargetType)0;

	private const bool shouldShowInCardLibrary = true;

	public override bool UsesSnowCrystals => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new YukiCrystalVar(2m)];

	public FallingSnow()
		: base(1, (CardType)2, (CardRarity)1, (TargetType)0, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		YukiCrystalSystem.AddCrystals((int)this.DynamicVars["YukiCrystal"].BaseValue);
		await Task.CompletedTask;
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiCrystal"].UpgradeValueBy(1m);
	}
}
