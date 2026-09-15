using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class CherishedMemories : YukiCardModel
{
	public override bool UsesEmpathy => true;

	protected override bool ShouldGlowGoldInternal
	{
		get
		{
			if (this.CombatState != null)
			{
				return this.CombatState.Enemies.Any((Creature e) => e != null && e.IsAlive && e.HasPower<EmpathyPower>());
			}
			return false;
		}
	}

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(8m, ValueProp.Move)];

	public CherishedMemories()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		bool targetHadEmpathy = cardPlay.Target.HasPower<EmpathyPower>();
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.Execute(choiceContext);
		if (!targetHadEmpathy)
		{
			return;
		}
		List<CardModel> list = PileTypeExtensions.GetPile(PileType.Discard, this.Owner).Cards.ToList();
		if (list.Count > 0)
		{
			LocString val = new LocString("ui", "TEXT_SELECT_CARD");
			CardSelectorPrefs val2 = new CardSelectorPrefs(val, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>)list, this.Owner, val2);
			if (enumerable != null && enumerable.Any())
			{
				await CardPileCmd.Add(enumerable.First(), PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}
