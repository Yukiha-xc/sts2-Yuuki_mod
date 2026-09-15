using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmpathyOfCourage : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

	public override string PortraitPath => "res://yuuki/images/cards/RINNE_e03a.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 1m)];

	public EmpathyOfCourage()
		: base(0, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int num = this.CombatState.Enemies.Count((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
		if (num > 0)
		{
			await PlayerCmd.GainEnergy((decimal)(num * 2), this.Owner);
		}
		int overloadCount = CapacityOverload;
		for (int i = 0; i < overloadCount; i++)
		{
			await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), (PileType)3, (Player)null, (CardPilePosition)1);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
	}
}

