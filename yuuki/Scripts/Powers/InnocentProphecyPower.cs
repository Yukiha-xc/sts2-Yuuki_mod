using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public class InnocentProphecyPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/InnocentProphecyPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/InnocentProphecyPower.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player != this.Owner.Player)
		{
			return;
		}
		this.Flash();
		await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner.Player);
		IReadOnlyList<CardModel> cards = PileType.Draw.GetPile(this.Owner.Player).Cards;
		if (cards.Any())
		{
			List<CardModel> cardsIn = (from c in cards
				orderby c.Rarity, c.Id
				select c).ToList();
			IEnumerable<CardModel> source = await CardSelectCmd.FromSimpleGrid(prefs: new CardSelectorPrefs(this.SelectionScreenPrompt, 1), context: choiceContext, cardsIn: cardsIn, player: this.Owner.Player);
			if (source.Any())
			{
				await CardPileCmd.Add(source.First(), PileType.Hand);
			}
		}
		await PowerCmd.Remove((PowerModel?)(object)this);
	}
}
