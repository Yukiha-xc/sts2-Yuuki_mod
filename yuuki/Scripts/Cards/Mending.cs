using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Mending : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/Mending.png";

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new PowerVar<VigorPower>(4m),
		new DynamicVar("YukiConsume", 1m)
	});

	protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat((IEnumerable<IHoverTip>)(object)new IHoverTip[1] { HoverTipFactory.FromPower<VigorPower>((int?)null) });

	public Mending()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (YukiCrystalSystem.CurrentCrystals >= 1)
		{
			await YukiCrystalSystem.AddCrystals(-1);
			int num = (int)this.DynamicVars["VigorPower"].BaseValue;
			await PowerCmd.Apply<VigorPower>(choiceContext, this.Owner.Creature, (decimal)num, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["VigorPower"].UpgradeValueBy(2m);
	}
}
