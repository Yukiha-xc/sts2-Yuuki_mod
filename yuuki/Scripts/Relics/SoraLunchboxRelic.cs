using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class SoraLunchboxRelic : CustomRelicModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    
    private bool _attackPlayed;
    private bool _skillPlayed;
    private bool _powerPlayed;
    private bool _statusPlayed;
    private bool _cursePlayed;

    [SavedProperty]
    public bool AttackPlayed
    {
        get => _attackPlayed;
        set { AssertMutable(); _attackPlayed = value; }
    }

    [SavedProperty]
    public bool SkillPlayed
    {
        get => _skillPlayed;
        set { AssertMutable(); _skillPlayed = value; }
    }

    [SavedProperty]
    public bool PowerPlayed
    {
        get => _powerPlayed;
        set { AssertMutable(); _powerPlayed = value; }
    }

    [SavedProperty]
    public bool StatusPlayed
    {
        get => _statusPlayed;
        set { AssertMutable(); _statusPlayed = value; }
    }

    [SavedProperty]
    public bool CursePlayed
    {
        get => _cursePlayed;
        set { AssertMutable(); _cursePlayed = value; }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/sora_lunchbox_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/sora_lunchbox_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/sora_lunchbox_relic.png";

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            AttackPlayed = false;
            SkillPlayed = false;
            PowerPlayed = false;
            StatusPlayed = false;
            CursePlayed = false;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!CombatManager.Instance.IsInProgress || cardPlay.Card.Owner != base.Owner)
            return;

        bool shouldGainEnergy = false;
        CardType type = cardPlay.Card.Type;

        switch (type)
        {
            case CardType.Attack:
                if (!AttackPlayed) { AttackPlayed = true; shouldGainEnergy = true; }
                break;
            case CardType.Skill:
                if (!SkillPlayed) { SkillPlayed = true; shouldGainEnergy = true; }
                break;
            case CardType.Power:
                if (!PowerPlayed) { PowerPlayed = true; shouldGainEnergy = true; }
                break;
            case CardType.Status:
                if (!StatusPlayed) { StatusPlayed = true; shouldGainEnergy = true; }
                break;
            case CardType.Curse:
                if (!CursePlayed) { CursePlayed = true; shouldGainEnergy = true; }
                break;
        }

        if (shouldGainEnergy)
        {
            Flash();
            await PlayerCmd.GainEnergy(1m, base.Owner);
        }
    }
}
