using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class PancakesRelic : CustomRelicModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    private int _turnCount;
    private int _cardsPlayedThisTurn;

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

    [SavedProperty]
    public int CardsPlayedThisTurn
    {
        get => _cardsPlayedThisTurn;
        set
        {
            AssertMutable();
            _cardsPlayedThisTurn = value;
        }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/pancakes_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/pancakes_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/pancakes_relic.png";

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            TurnCount = 0;
            CardsPlayedThisTurn = 0;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner)
        {
            TurnCount++;
            CardsPlayedThisTurn = 0;

            if (TurnCount == 1)
            {
                Flash();
                await PlayerCmd.GainEnergy(1m, player);
            }
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!CombatManager.Instance.IsInProgress || cardPlay.Card.Owner != base.Owner)
            return;

        CardsPlayedThisTurn++;
        
        if (CardsPlayedThisTurn == 3)
        {
            Flash();
            await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, null);
        }
    }
}
