using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public class FimbulwinterPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/fenbu.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/fenbu.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player)
		{
			this.Flash();
			await YukiCrystalSystem.AddCrystals();
			int amount = this.Amount;
			if (YukiCrystalSystem.CurrentCrystals <= amount)
			{
				await YukiCrystalSystem.AddCrystals();
			}
		}
	}
}
