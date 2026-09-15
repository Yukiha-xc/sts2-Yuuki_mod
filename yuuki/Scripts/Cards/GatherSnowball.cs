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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class GatherSnowball : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/GatherSnowball.png";

	public override bool UsesSnowCrystals => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new YukiCrystalVar(1m)];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat((IEnumerable<IHoverTip>)(object)new IHoverTip[1] { HoverTipFactory.FromPower<GatherSnowballPower>((int?)null) });

	public GatherSnowball()
		: base(1, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
		YukiCrystalSystem.AddCrystals((int)this.DynamicVars["YukiCrystal"].BaseValue);
		await PowerCmd.Apply<GatherSnowballPower>(choiceContext, this.Owner.Creature, 2m, this.Owner.Creature, (CardModel)this, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiCrystal"].UpgradeValueBy(1m);
	}
}
