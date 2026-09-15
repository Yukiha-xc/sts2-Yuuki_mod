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
public class MagicOfAbility : YukiCardModel
{
	public override int CapacityOverload => 1;

	public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e702a.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Damage", 9m)];

	public MagicOfAbility()
		: base(1, (CardType)3, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		PowerCmd.Apply<MagicOfAbilityPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Damage"].BaseValue, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Damage"].UpgradeValueBy(2m);
		this.AddKeyword((CardKeyword)3);
	}
}
