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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class RegainHeart : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/KORONA_FD_e01b.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new IntVar("DrawCount", 3m),
		(DynamicVar)new IntVar("ExtraDraw", 1m)
	});

	public RegainHeart()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)0, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = (int)this.DynamicVars["DrawCount"].BaseValue;
		await CardPileCmd.Draw(choiceContext, (decimal)num, this.Owner, false);
		Creature val = this.CombatState.Enemies.FirstOrDefault((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
		if (val != null)
		{
			await PowerCmd.Remove<EmpathyPower>(val);
			int num2 = (int)this.DynamicVars["ExtraDraw"].BaseValue;
			await CardPileCmd.Draw(choiceContext, (decimal)num2, this.Owner, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["ExtraDraw"].UpgradeValueBy(1m);
	}
}
