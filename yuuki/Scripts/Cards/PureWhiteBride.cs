using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PureWhiteBride : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override string PortraitPath => "res://yuuki/images/cards/xiangu2.png";

	public override IEnumerable<CardKeyword> CanonicalKeywords => this.IsUpgraded ? [CardKeyword.Ethereal, CardKeyword.Innate] : [CardKeyword.Ethereal];

	public PureWhiteBride()
		: base(0, (CardType)2, (CardRarity)5, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		foreach (Creature enemy in this.CombatState.Enemies)
		{
			if (enemy.IsAlive)
			{
				await PowerCmd.Apply<EmpathyPower>(choiceContext, enemy, 1m, this.Owner.Creature, (CardModel)this, false);
			}
		}
		List<CardModel> list = GetVoids(this.Owner).ToList();
		foreach (CardModel item in list)
		{
			await CardCmd.Exhaust(choiceContext, item, false, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	private IEnumerable<CardModel> GetVoids(Player owner)
	{
		return owner.PlayerCombatState.AllCards.Where((CardModel c) => c is MegaCrit.Sts2.Core.Models.Cards.Void && c.Pile != null && (int)c.Pile.Type != 4);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
		this.AddKeyword(CardKeyword.Innate);
	}
}


