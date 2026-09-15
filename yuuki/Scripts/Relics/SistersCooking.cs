using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class SistersCooking : CustomRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Uncommon;

	public override string PackedIconPath => "res://yuuki/images/relics/cook.png";

	protected override string PackedIconOutlinePath => "res://yuuki/images/relics/cook.png";

	protected override string BigIconPath => "res://yuuki/images/relics/cook.png";

	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner == this.Owner && cardPlay.Card.Type == CardType.Power)
		{
			this.Flash();
			await YukiCrystalSystem.AddCrystals();
		}
	}

	public SistersCooking()
		: base(true)
	{
	}
}
