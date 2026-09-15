using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public sealed class PsychicGuardianPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/PsychicGuardianPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/PsychicGuardianPower.png";

	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (this.Owner.Player is { } owner && card.Owner.Creature == this.Owner && (card.GetType().Name.Contains("Void") || card.Id.Entry == "Void"))
		{
			this.Flash();
			await PlayerCmd.GainEnergy(this.Amount, owner);
			await CardPileCmd.Draw(choiceContext, this.Amount, owner);
		}
	}
}
