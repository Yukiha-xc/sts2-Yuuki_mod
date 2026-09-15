using System.Collections.Generic;
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
public class SnowFairyChant : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/YUKI_e03a6.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new PowerVar<SnowFairyChantPower>("MagicNumber", 1m)];

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			foreach (IHoverTip extraHoverTip in base.ExtraHoverTips)
			{
				yield return extraHoverTip;
			}
			yield return HoverTipFactory.FromPower<SnowFairyChantPower>((int?)null);
		}
	}

	public SnowFairyChant()
		: base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<SnowFairyChantPower>(choiceContext, this.Owner.Creature, this.DynamicVars["MagicNumber"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["MagicNumber"].UpgradeValueBy(1m);
	}
}
