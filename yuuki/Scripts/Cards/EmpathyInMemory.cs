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
public class EmpathyInMemory : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override string PortraitPath => "res://yuuki/images/cards/YUKI_e01a2.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(8m, (ValueProp)8)];

	public EmpathyInMemory()
		: base(2, (CardType)3, (CardRarity)3, (TargetType)0, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<EmpathyInMemoryPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}
