using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EndOfWandering : YukiCardModel
{
	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 2m)];

	public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>((CardKeyword[])(object)new CardKeyword[2]
	{
		(CardKeyword)1,
		(CardKeyword)5
	});

	public EndOfWandering()
		: base(0, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		foreach (CardModel card in PileTypeExtensions.GetPile((PileType)2, this.Owner).Cards)
		{
			card.AddKeyword((CardKeyword)5);
		}
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

