using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class HeartKnot : YukiCardModel
{
	public override bool UsesEmpathy => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(18m, (ValueProp)8),
		(DynamicVar)new EnergyVar(1)
	});

	public HeartKnot()
		: base(3, (CardType)1, (CardRarity)3, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<Creature> list = this.CombatState.Enemies.Where((Creature e) => e != null && e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
		int count = list.Count;
		foreach (Creature item in list)
		{
			await PowerCmd.Remove<EmpathyPower>(item);
		}
		int totalTriggers = 1 + count;
		for (int i = 0; i < totalTriggers; i++)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
				.Execute(choiceContext);
			await PlayerCmd.GainEnergy(1m, this.Owner);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(4m);
	}
}
