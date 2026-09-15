using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class OverloadResponse : YukiCardModel
{
	public override int CapacityOverload => 1;

	public OverloadResponse()
		: base(3, (CardType)3, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<OverloadResponsePower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}
