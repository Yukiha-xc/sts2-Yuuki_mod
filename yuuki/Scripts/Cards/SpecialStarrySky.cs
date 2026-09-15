using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialStarrySky : YukiCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>((CardKeyword[])(object)new CardKeyword[2]
	{
		(CardKeyword)5,
		(CardKeyword)1
	});

	public override string PortraitPath => "res://yuuki/images/cards/StarrySky.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new IntVar("Doom", 15m),
		(DynamicVar)new IntVar("Energy", 2m)
	});

	public SpecialStarrySky()
		: base(0, (CardType)2, (CardRarity)7, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		foreach (Creature hittableEnemy in this.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<DoomPower>(choiceContext, hittableEnemy, this.DynamicVars["Doom"].BaseValue, this.Owner.Creature, (CardModel)this, false);
		}
		await PlayerCmd.GainEnergy((decimal)(int)this.DynamicVars["Energy"].BaseValue, this.Owner);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Doom"].UpgradeValueBy(5m);
	}
}
