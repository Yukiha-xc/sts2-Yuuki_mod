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
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class MusicBoxRelic : CustomRelicModel
{
    private int _triggersThisTurn;

    [SavedProperty]
    public int TriggersThisTurn
    {
        get => _triggersThisTurn;
        set { AssertMutable(); _triggersThisTurn = value; }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/music_box_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/music_box_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/music_box_relic.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    };

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            TriggersThisTurn = 0;
        }
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner)
        {
            TriggersThisTurn = 0;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!CombatManager.Instance.IsInProgress || cardPlay.Card.Owner != base.Owner)
            return;

        if (TriggersThisTurn >= 2) return;

        if (cardPlay.Card.Type == CardType.Skill)
        {
            Flash();
            TriggersThisTurn++;
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 1m, base.Owner.Creature, null);
        }
        else if (cardPlay.Card.Type == CardType.Attack)
        {
            Flash();
            TriggersThisTurn++;
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 1m, base.Owner.Creature, null);
        }
    }
}
