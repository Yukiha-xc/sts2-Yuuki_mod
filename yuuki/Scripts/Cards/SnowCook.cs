using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCook : YukiCardModel
{
	public override bool GainsBlock => true;

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new BlockVar(8m, (ValueProp)8),
		new YukiCrystalVar(1m)
	});

	public SnowCook()
		: base(1, (CardType)2, (CardRarity)2, (TargetType)0, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay, false);
		YukiCrystalSystem.AddCrystals();
		await Task.CompletedTask;
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
	}
}
