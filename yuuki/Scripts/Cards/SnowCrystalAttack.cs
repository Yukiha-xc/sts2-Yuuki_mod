using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCrystalAttack : YukiCardModel, ITranscendenceCard
{
	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new CrystalDamageVar(),
		new DynamicVar("YukiConsume", 2m)
	});

	public SnowCrystalAttack()
		: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	public CardModel GetTranscendenceTransformedCard()
	{
		return (CardModel)ModelDb.Card<WhiteOath>();
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target is not { } target)
		{
			return;
		}

		await DamageCmd.Attack(((DynamicVar)(DamageVar)this.DynamicVars["Damage"]).BaseValue).FromCard(this, cardPlay).Targeting(target)
			.Execute(choiceContext);
		await DamageCmd.Attack(((DynamicVar)(DamageVar)this.DynamicVars["Damage"]).BaseValue).FromCard(this, cardPlay).Targeting(target)
			.Execute(choiceContext);
		int num = (int)this.DynamicVars["YukiConsume"].BaseValue;
		if (num > 0)
		{
			await YukiCrystalSystem.AddCrystals(-num);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiConsume"].UpgradeValueBy(-1m);
	}
}
