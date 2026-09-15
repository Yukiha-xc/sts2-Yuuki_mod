using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ResonanceImpact : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override int CapacityOverload => 1;

	public override string PortraitPath => "res://yuuki/images/cards/BG081_125.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(13m, (ValueProp)8)];

	public ResonanceImpact()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target != null)
		{
			bool targetHadEmpathy = cardPlay.Target.HasPower<EmpathyPower>();
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).Targeting(cardPlay.Target)
				.Execute(choiceContext);
			if (targetHadEmpathy)
			{
				await PowerCmd.Remove<EmpathyPower>(cardPlay.Target);
				CardModel obj = this.CreateClone();
				obj.EnergyCost.SetThisCombat(0, false);
				await CardPileCmd.AddGeneratedCardToCombat(obj, (PileType)3, (Player)null, (CardPilePosition)1);
			}
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}
