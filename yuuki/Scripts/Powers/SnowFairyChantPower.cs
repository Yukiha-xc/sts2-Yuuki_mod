using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public sealed class SnowFairyChantPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowFairyChantPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/SnowFairyChantPower.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player && YukiCrystalSystem.CurrentCrystals >= 1)
		{
			await YukiCrystalSystem.AddCrystals(-1);
			this.Flash();
			int amount = this.Amount;
			await CardPileCmd.Draw(choiceContext, amount, player);
		}
	}
}
