using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Models;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class WishTagRelic : CustomRelicModel
{
    private bool _activatedThisCombat;

    [SavedProperty]
    public bool ActivatedThisCombat
    {
        get => _activatedThisCombat;
        set
        {
            AssertMutable();
            _activatedThisCombat = value;
        }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(1m)];

    public override string PackedIconPath => "res://yuuki/images/relics/wish_tag_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/wish_tag_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/wish_tag_relic.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<BufferPower>()
    };

    public override Task BeforeCombatStart()
    {
        ActivatedThisCombat = false;
        return Task.CompletedTask;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (!CombatManager.Instance.IsInProgress || card.Owner != base.Owner)
            return;

        if (!ActivatedThisCombat)
        {
            Flash();
            ActivatedThisCombat = true;
            
            // The first exhausted card each combat grants Buffer and Max HP.
            await PowerCmd.Apply<BufferPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, card);
            await CreatureCmd.GainMaxHp(base.Owner.Creature, 2m);
        }
    }
}



