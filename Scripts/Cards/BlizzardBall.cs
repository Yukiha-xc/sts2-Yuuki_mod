using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace yuuki.Scripts.Cards;

public class BlizzardBallDamageVar : DamageVar
{
    public BlizzardBallDamageVar(decimal baseVal) : base(baseVal, ValueProp.Move) { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        // 仅在战斗中应用雪晶加成
        bool isInCombat = card.CombatState != null;
        int crystals = isInCombat ? YukiCrystalSystem.CurrentCrystals : 0;
        
        decimal originalBase = card.IsUpgraded ? 13m : 9m;
        this.BaseValue = originalBase + (crystals * 2m);
        base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
    }
}

[Pool(typeof(YukiPool))]
public class BlizzardBall : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/BlizzardSnowball.png";

    public BlizzardBall() : base(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlizzardBallDamageVar(9m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
