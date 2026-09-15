using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class MoonstoneRelic : CustomRelicModel
{
    private bool _usedThisCombat;

    [SavedProperty]
    public bool UsedThisCombat
    {
        get => _usedThisCombat;
        set
        {
            AssertMutable();
            _usedThisCombat = value;
        }
    }

    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(2m), new PowerVar<IntangiblePower>(2m)];

    public override string PackedIconPath => "res://yuuki/images/relics/moonstone_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/moonstone_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/moonstone_relic.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<IntangiblePower>()
    };

    public override async Task BeforeCombatStart()
    {
        UsedThisCombat = false;
        Flash();
        await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 2m, base.Owner.Creature, null);
    }

    public override bool ShouldDieLate(Creature creature)
    {
        if (creature != base.Owner.Creature) return true;
        if (UsedThisCombat) return true;
        if (creature.MaxHp <= 30) return true;
        return false;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        UsedThisCombat = true;
        
        await CreatureCmd.Heal(creature, 25m);
        
        decimal loss = Math.Min(30m, Math.Max(0m, (decimal)creature.MaxHp - 1m));
        if (loss > 0m)
        {
            await CreatureCmd.SetMaxHp(creature, (decimal)creature.MaxHp - loss);
        }

        await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), creature, 2m, creature, null);
    }
}


