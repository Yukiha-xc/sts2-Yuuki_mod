using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Picnic : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/YUUKI_FD_e02a.png";

	public override bool UsesEmpathy => true;

	public override int CapacityOverload => 1;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(15m, ValueProp.Move)];

	public Picnic()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		List<Creature> list = combatState.HittableEnemies.ToList();
		if (list.All((Creature e) => e.HasPower<EmpathyPower>()) && list.Count > 0)
		{
			int count = list.Count;
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).WithHitCount(count)
				.TargetingAllOpponents(combatState)
				.Execute(choiceContext);
		}
		else
		{
			foreach (Creature item in list)
			{
				await PowerCmd.Apply<EmpathyPower>(choiceContext, item, 1m, this.Owner.Creature, (CardModel)this, false);
			}
			await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}

