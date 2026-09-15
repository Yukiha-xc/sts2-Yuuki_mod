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
public class PakuPaku : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/PakuPaku.png";

	public override bool UsesSnowCrystals => true;

	public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
	{
		new DynamicVar("YukiConsume", 1m),
		new CardsVar(1),
		new DynamicVar("Energy", 1m)
	};

	public PakuPaku()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.None, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = (int)this.DynamicVars["YukiConsume"].BaseValue;
		if (YukiCrystalSystem.CurrentCrystals >= num)
		{
			await YukiCrystalSystem.AddCrystals(-num);
			await PlayerCmd.GainEnergy(this.DynamicVars["Energy"].BaseValue, this.Owner);
			await CardPileCmd.Draw(choiceContext, this.DynamicVars["Cards"].BaseValue, this.Owner, false);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Cards"].UpgradeValueBy(1m);
		this.DynamicVars["Energy"].UpgradeValueBy(1m);
	}
}
