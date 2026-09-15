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
public class MeltingSnow : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/SD_FD_011a.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new BlockVar(7m, (ValueProp)8),
		new YukiCrystalVar(1m)
	});

	public MeltingSnow()
		: base(1, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = (int)this.DynamicVars["YukiCrystal"].BaseValue;
		if (YukiCrystalSystem.CurrentCrystals >= num)
		{
			YukiCrystalSystem.AddCrystals(-num);
			await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, (ValueProp)8, cardPlay, false);
			await PowerCmd.Apply<MeltingSnowPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
	}
}
