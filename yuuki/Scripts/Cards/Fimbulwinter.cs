using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Fimbulwinter : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/Fimbulwinter_Portrait.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Threshold", 4m)];

	public Fimbulwinter()
		: base(1, (CardType)3, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<FimbulwinterPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Threshold"].BaseValue, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Threshold"].UpgradeValueBy(1m);
	}
}
