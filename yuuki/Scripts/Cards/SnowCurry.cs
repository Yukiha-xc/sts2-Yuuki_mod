using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCurry : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e004a.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("YukiConsume", 3m)];

	protected override bool IsPlayable => YukiCrystalSystem.CurrentCrystals >= (int)this.DynamicVars["YukiConsume"].BaseValue;

	protected override bool ShouldGlowGoldInternal => this.IsPlayable;

	public SnowCurry()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = (int)this.DynamicVars["YukiConsume"].BaseValue;
		if (YukiCrystalSystem.CurrentCrystals >= num)
		{
			await YukiCrystalSystem.AddCrystals(-num);
			await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
			await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiConsume"].UpgradeValueBy(-1m);
	}
}

