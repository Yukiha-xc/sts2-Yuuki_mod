using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class CapacityRing : CustomRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;

protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];

	public override string PackedIconPath => "res://yuuki/images/relics/capacity_ring.png";

	protected override string PackedIconOutlinePath => "res://yuuki/images/relics/capacity_ring.png";

	protected override string BigIconPath => "res://yuuki/images/relics/capacity_ring.png";

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			foreach (IHoverTip extraHoverTip in base.ExtraHoverTips)
			{
				yield return extraHoverTip;
			}
			yield return HoverTipFactory.ForEnergy((RelicModel)(object)this);
			yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>();
			yield return new HoverTip(new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"), new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"));
		}
	}

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		ICombatState? combatState = player.Creature.CombatState;
		if (player == this.Owner && combatState is not null && combatState.RoundNumber <= 1)
		{
			this.Flash();
			await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
			await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null);
		}
	}

	public CapacityRing()
		: base(true)
	{
	}
}
