using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PowerDefense : YukiCardModel
{
public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

	public PowerDefense()
		: base(0, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<CardModel> list = PileTypeExtensions.GetPile((PileType)2, this.Owner).Cards.Where((CardModel c) => (int)c.Type == 4 || (int)c.Type == 5).ToList();
		int count = list.Count;
		if (count > 0)
		{
			foreach (CardModel item in list)
			{
				await CardPileCmd.Add(item, (PileType)4, (CardPilePosition)1, (AbstractModel)null, false);
			}
			await CardPileCmd.Draw(choiceContext, (decimal)count, this.Owner, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.AddKeyword((CardKeyword)5);
	}
}
