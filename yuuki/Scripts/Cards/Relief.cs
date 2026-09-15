using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Relief : YukiCardModel
{
	public class ReliefBlockVar : BlockVar
	{
		public ReliefBlockVar(decimal baseVal)
			: base(baseVal, (ValueProp)8)
		{
		}

		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
		{
			decimal num = (card.IsUpgraded ? 11m : 9m);
			bool flag = false;
			if (card.CombatState != null)
			{
				flag = card.CombatState.Enemies.Any((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
			}
			decimal baseValue = (flag ? (num + 4m) : num);
			this.BaseValue = baseValue;
			base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
		}
	}

	public override bool GainsBlock => true;

	public override bool UsesEmpathy => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new ReliefBlockVar(9m)];

	public Relief()
		: base(1, (CardType)2, (CardRarity)2, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, (ValueProp)8, cardPlay, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(2m);
	}
}
