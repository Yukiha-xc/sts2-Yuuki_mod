using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class NightWatchRelic : CustomRelicModel
{
    private int _cardsPlayedThisCombat;

    [SavedProperty]
    public int CardsPlayedThisCombat
    {
        get => _cardsPlayedThisCombat;
        set 
        { 
            AssertMutable(); 
            _cardsPlayedThisCombat = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/night_watch_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/night_watch_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/night_watch_relic.png";

    public override bool ShowCounter => true;
    public override int DisplayAmount => CardsPlayedThisCombat;

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            CardsPlayedThisCombat = 0;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!CombatManager.Instance.IsInProgress || cardPlay.Card.Owner != base.Owner)
            return;

        CardsPlayedThisCombat++;
        
        if (CardsPlayedThisCombat == 12)
        {
            Flash();
            var combatState = base.Owner.Creature.CombatState;
            if (combatState != null)
            {
                foreach (Creature enemy in combatState.Enemies)
                {
                    if (!enemy.IsDead)
                    {
                        await CreatureCmd.Stun(enemy);
                    }
                }
            }
        }
    }
}
