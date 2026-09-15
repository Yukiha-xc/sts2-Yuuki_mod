using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Prayer : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override string PortraitPath => "res://yuuki/images/cards/SYUKI_FD_e06a2.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(3m, (ValueProp)8),
		new DynamicVar("Hits", 2m)
	});

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			foreach (IHoverTip extraHoverTip in base.ExtraHoverTips)
			{
				yield return extraHoverTip;
			}
			yield return HoverTipFactory.FromPower<PrayerPower>((int?)null);
		}
	}

	public Prayer()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int hits = (int)this.DynamicVars["Hits"].BaseValue;
		for (int i = 0; i < hits; i++)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
				.Execute(choiceContext);
		}
		await PowerCmd.Apply<PrayerPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Hits"].UpgradeValueBy(1m);
	}
}
