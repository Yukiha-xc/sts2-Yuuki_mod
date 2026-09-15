using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class MageDiaryRelic : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/mage_diary_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/mage_diary_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/mage_diary_relic.png";

protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    private int _turnCount;

    [SavedProperty]
    public int TurnCount
    {
        get => _turnCount;
        set
        {
            AssertMutable();
            _turnCount = value;
        }
    }

    public override Task AfterRoomEntered(MegaCrit.Sts2.Core.Rooms.AbstractRoom room)
    {
        if (room is MegaCrit.Sts2.Core.Rooms.CombatRoom)
        {
            TurnCount = 0;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner)
        {
            TurnCount++;
            if (TurnCount <= 3)
            {
                Flash();
                
                await CardPileCmd.Draw(choiceContext, 1, player);
                
                CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 0, 1);
                CardModel? cardToExhaust = (await CardSelectCmd.FromHand(prefs: prefs, context: choiceContext, player: player, filter: null, source: this)).FirstOrDefault();
                
                if (cardToExhaust != null)
                {
                    await CardCmd.Exhaust(choiceContext, cardToExhaust);
                    await PlayerCmd.GainEnergy(1m, player);
                }
            }
        }
    }
}
