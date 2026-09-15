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
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Partner : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override string PortraitPath => "res://yuuki/images/cards/RINNE_e10b.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new CardsVar(1)];

	public Partner()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CardPileCmd.Draw(choiceContext, ((DynamicVar)this.DynamicVars.Cards).BaseValue, this.Owner, false);
		int num = this.CombatState.Enemies.Count((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
		if (num > 0)
		{
			await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, (decimal)num, this.Owner.Creature, (CardModel)this, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}
