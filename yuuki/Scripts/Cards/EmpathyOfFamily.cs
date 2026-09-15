using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmpathyOfFamily : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)2];

	public EmpathyOfFamily()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target != null)
		{
			await PowerCmd.Apply<EmpathyPower>(choiceContext, cardPlay.Target, 1m, this.Owner.Creature, (CardModel)this, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}
