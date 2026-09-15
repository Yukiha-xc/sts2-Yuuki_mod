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
public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 2m)];

	public MechanicalHeartbeat()
		: base(1, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<ArtifactPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
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

