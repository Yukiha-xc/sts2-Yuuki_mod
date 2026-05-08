using BaseLib.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class MagicOfAbilityPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            yield return new HoverTip(
                new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"),
                new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"),
                null
            );
            yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>();
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            
            if (!base.CombatState.Enemies.Any(e => e.IsAlive)) return;

            Flash();
            
            if (base.Amount > 0)
            {
                await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, (decimal)base.Amount, ValueProp.Move, base.Owner);
            }

            
            if (!base.CombatState.Enemies.Any(e => e.IsAlive)) return;

            
            await PlayerCmd.GainEnergy(1, player);

            
            CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(player);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
            await Cmd.Wait(0.25f);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/MagicOfAbilityPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/MagicOfAbilityPower.png";
}

