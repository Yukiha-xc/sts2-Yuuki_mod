using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmergencyDefense : YukiCardModel
{
	public override bool GainsBlock => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new BlockVar(7m, (ValueProp)8)];

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			foreach (IHoverTip extraHoverTip in base.ExtraHoverTips)
			{
				yield return extraHoverTip;
			}
			yield return HoverTipFactory.FromCard<Dazed>(false);
		}
	}

	public EmergencyDefense()
		: base(0, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, (ValueProp)8, cardPlay, false);
		if (!this.CombatState.Enemies.Any((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>()))
		{
			CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<Dazed>(this.Owner), (PileType)3, (Player)null, (CardPilePosition)1), 1.2f, (CardPreviewStyle)1);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(3m);
	}
}
