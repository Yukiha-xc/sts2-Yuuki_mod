using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class MagicOfAbilityPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return new HoverTip(new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"), new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"));
			yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>();
		}
	}

	public override string CustomPackedIconPath => "res://yuuki/images/powers/MagicOfAbilityPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/MagicOfAbilityPower.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player && this.CombatState is { } combatState && combatState.Enemies.Any((Creature e) => e.IsAlive))
		{
			this.Flash();
			if (this.Amount > 0)
			{
				await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, this.Amount, ValueProp.Move, this.Owner);
			}
			if (combatState.Enemies.Any((Creature e) => e.IsAlive))
			{
				await PlayerCmd.GainEnergy(1m, player);
				await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(player), PileType.Discard, null);
				await Cmd.Wait(0.25f);
			}
		}
	}
}
