using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class SecretTreasure : CustomRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;

	public override string PackedIconPath => "res://yuuki/images/relics/relics1.png";

	protected override string PackedIconOutlinePath => "res://yuuki/images/relics/relics1.png";

	protected override string BigIconPath => "res://yuuki/images/relics/relics1.png";

	public override async Task BeforeCombatStart()
	{
		ICombatState combatState = this.Owner.Creature.CombatState;
		if (combatState == null)
		{
			return;
		}
		List<Creature> list = combatState.HittableEnemies.ToList();
		if (list.Count > 0)
		{
			Creature creature = this.Owner.RunState.Rng.CombatTargets.NextItem(list);
			if (creature != null)
			{
				this.Flash();
				await PowerCmd.Apply<EmpathyPower>((PlayerChoiceContext)new ThrowingPlayerChoiceContext(), creature, 1m, this.Owner.Creature, (CardModel?)null, false);
			}
		}
	}

	public SecretTreasure()
		: base(true)
	{
	}
}
