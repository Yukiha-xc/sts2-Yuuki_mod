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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Reunion : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override string PortraitPath => "res://yuuki/images/cards/OCHIBA_e04b2.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new EnergyVar(1)];

	public Reunion()
		: base(0, (CardType)2, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PlayerCmd.GainEnergy(1m, this.Owner);
		List<Creature> list = this.CombatState.Enemies.Where((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
		if (list.Count > 0)
		{
			foreach (Creature item in list)
			{
				await PowerCmd.Remove<EmpathyPower>(item);
			}
			await PlayerCmd.GainEnergy((decimal)(int)this.DynamicVars["Energy"].BaseValue, this.Owner);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Energy"].UpgradeValueBy(1m);
	}
}
