using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MechanicalHeartbeat : YukiCardModel
{
public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 2m)];

	public MechanicalHeartbeat()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await PowerCmd.Apply<ArtifactPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		int overloadCount = CapacityOverload;
		for (int i = 0; i < overloadCount; i++)
		{
			await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
	}
}

