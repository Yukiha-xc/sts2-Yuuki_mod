using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class AboutToMelt : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/YUKI_e15d.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(15m, ValueProp.Move)];

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			foreach (IHoverTip extraHoverTip in base.ExtraHoverTips)
			{
				yield return extraHoverTip;
			}
			yield return HoverTipFactory.FromCard<Burn>(false);
		}
	}

	public AboutToMelt()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target is not { } target || this.CombatState is not { } combatState)
		{
			return;
		}

		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(target)
			.Execute(choiceContext);
		if (YukiCrystalSystem.CurrentCrystals < 5)
		{
			CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<Burn>(this.Owner), PileType.Draw, null, CardPilePosition.Bottom), 1.2f, CardPreviewStyle.HorizontalLayout);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(5m);
	}
}
