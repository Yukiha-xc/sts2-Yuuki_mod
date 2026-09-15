using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public sealed class MeltingSnowPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/MeltingSnowPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/MeltingSnowPower.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player)
		{
			this.Flash();
			int num = this.Amount * 2;
			await CardPileCmd.Draw(choiceContext, num, player);
			await PowerCmd.Remove((PowerModel?)(object)this);
		}
	}
}
