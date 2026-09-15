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
public class WhiteOath : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/WhiteOath.png";

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new MultiplierVar(),
		(DynamicVar)new WhiteOathDamageVar()
	});

	public WhiteOath()
		: base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target is not { } target)
		{
			return;
		}

		await YukiCrystalSystem.AddCrystals(2);
		((DynamicVar)this.DynamicVars.Damage).UpdateCardPreview(this, CardPreviewMode.Normal, target, true);
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(target)
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Multiplier"].UpgradeValueBy(1m);
	}
}
