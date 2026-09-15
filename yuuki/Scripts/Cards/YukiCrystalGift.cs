using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class YukiCrystalGift : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

	public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e005a.png";

public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new YukiCrystalVar(2m)];

	public YukiCrystalGift()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		CardPile drawPile = PileTypeExtensions.GetPile(PileType.Draw, this.Owner);
		await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner);
		CardModel? val = drawPile.Cards.FirstOrDefault();
		if (val != null)
		{
			await CardCmd.Exhaust(choiceContext, val, false, false);
		}
		await PlayerCmd.GainEnergy(1m, this.Owner);
		await YukiCrystalSystem.AddCrystals((int)this.DynamicVars["YukiCrystal"].BaseValue);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiCrystal"].UpgradeValueBy(1m);
	}
}
