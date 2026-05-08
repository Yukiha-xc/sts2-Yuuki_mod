using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using yuuki.Scripts;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class CapacityRing : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    
    public override string PackedIconPath => "res://yuuki/images/relics/capacity_ring.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/capacity_ring.png";
    protected override string BigIconPath => "res://yuuki/images/relics/capacity_ring.png";

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            
            foreach (var tip in base.ExtraHoverTips) yield return tip;
            
            
            yield return HoverTipFactory.ForEnergy(this);
            
            
            yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>();
            
            
            yield return new HoverTip(
                new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"),
                new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"),
                null
            );
        }
    }

    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        
        var combatState = player.Creature.CombatState;

        
        if (player == base.Owner && combatState.RoundNumber <= 1)
        {
            this.Flash();
            
            
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            
            
            CardModel voidCard = combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
        }
    }
}

