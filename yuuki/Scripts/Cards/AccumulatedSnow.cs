using System.Collections.Generic;
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
public class AccumulatedSnow : YukiCardModel
{
	public override bool UsesEmpathy => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[3]
	{
		(DynamicVar)new DamageVar(3m, (ValueProp)8),
		(DynamicVar)new IntVar("ExtraDamage", 8m),
		new YukiCrystalVar(1m)
	});

	public AccumulatedSnow()
		: base(0, (CardType)1, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		Creature target = cardPlay.Target;
		if (target != null)
		{
			bool num = target.HasPower<EmpathyPower>();
			decimal damageToDeal = ((DynamicVar)this.DynamicVars.Damage).BaseValue;
			if (num)
			{
				await PowerCmd.Remove<EmpathyPower>(target);
				damageToDeal += this.DynamicVars["ExtraDamage"].BaseValue;
				YukiCrystalSystem.AddCrystals();
			}
			if (target.IsAlive)
			{
				await DamageCmd.Attack(damageToDeal).FromCard(this).Targeting(target)
					.Execute(choiceContext);
			}
			await Cmd.Wait(0.25f, false);
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
		this.DynamicVars["ExtraDamage"].UpgradeValueBy(4m);
	}
}
