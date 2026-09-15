using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class InnocentProphecy : YukiCardModel
{
	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 2m)];

	public InnocentProphecy()
		: base(1, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner);
		IReadOnlyList<CardModel> cards = PileTypeExtensions.GetPile((PileType)1, this.Owner).Cards;
		if (cards.Any())
		{
			CardSelectorPrefs val = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> source = await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>)cards.ToList(), this.Owner, val);
			if (source.Any())
			{
				await CardPileCmd.Add(source.First(), (PileType)2, (CardPilePosition)1, (AbstractModel)null, false);
			}
		}
		await PowerCmd.Apply<InnocentProphecyPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		int overloadCount = CapacityOverload;
		for (int i = 0; i < overloadCount; i++)
		{
			await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), (PileType)3, (Player)null, (CardPilePosition)1);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
	}
}



