using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class FoundYou : YukiCardModel
{
	public override bool UsesEmpathy => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Discard", 2m)];

	public FoundYou()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CardPileCmd.Draw(choiceContext, 2m, this.Owner, false);
		int num = (int)this.DynamicVars["Discard"].BaseValue;
		List<CardModel> selectedCards = (await CardSelectCmd.FromHandForDiscard(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, num), null, this)).ToList();
		if (selectedCards.Count > 0)
		{
			await CardCmd.Discard(choiceContext, (IEnumerable<CardModel>)selectedCards);
			if (selectedCards.All((CardModel c) => (int)c.Type == 2) && cardPlay.Target != null)
			{
				await PowerCmd.Apply<EmpathyPower>(choiceContext, cardPlay.Target, 1m, this.Owner.Creature, (CardModel)this, false);
			}
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Discard"].UpgradeValueBy(-1m);
	}
}
