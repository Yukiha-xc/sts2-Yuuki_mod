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
public class SnowDaughter : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new PowerVar<SnowDaughterPower>("MagicNumber", 2m)];

	public SnowDaughter()
		: base(1, (CardType)3, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
		YukiCrystalSystem.AddCrystals(2);
		await PowerCmd.Apply<SnowDaughterPower>(choiceContext, this.Owner.Creature, this.DynamicVars["MagicNumber"].BaseValue, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["MagicNumber"].UpgradeValueBy(2m);
	}
}
