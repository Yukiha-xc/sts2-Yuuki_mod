using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCrystalAbsorption : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new DynamicVar("YukiConsume", 2m),
		new DynamicVar("EnergyGain", 2m)
	});

	public SnowCrystalAbsorption()
		: base(0, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = (int)this.DynamicVars["YukiConsume"].BaseValue;
		if (YukiCrystalSystem.CurrentCrystals >= num)
		{
			YukiCrystalSystem.AddCrystals(-num);
			await PlayerCmd.GainEnergy((decimal)(int)this.DynamicVars["EnergyGain"].BaseValue, this.Owner);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["EnergyGain"].UpgradeValueBy(1m);
	}
}
