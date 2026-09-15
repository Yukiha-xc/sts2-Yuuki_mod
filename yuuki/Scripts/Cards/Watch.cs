using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Watch : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override bool GainsBlock => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new BlockVar(5m, (ValueProp)8)];

	public Watch()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, (ValueProp)8, cardPlay, false);
		if (cardPlay.Target != null)
		{
			await PowerCmd.Apply<EmpathyPower>(choiceContext, cardPlay.Target, 1m, this.Owner.Creature, (CardModel)this, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
	}
}
