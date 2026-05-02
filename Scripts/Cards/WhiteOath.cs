using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

public class WhiteOathDamageVar : DamageVar
{
    public WhiteOathDamageVar() : base(0, ValueProp.Move) { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        int crystals = YukiCrystalSystem.CurrentCrystals;
        decimal multiplier = card.IsUpgraded ? 5m : 4m;
        decimal finalBaseValue = crystals * multiplier;
        this.BaseValue = finalBaseValue;
        base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
    }
}

public class MultiplierVar : DynamicVar
{
    public MultiplierVar() : base("Multiplier", 4m) { }
}

[Pool(typeof(YukiPool))]
public class WhiteOath : YukiCardModel
{
    public override string PortraitPath => "res://yuuki/images/cards/WhiteOath.png";

    public WhiteOath() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new WhiteOathDamageVar(),
        new MultiplierVar()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        YukiCrystalSystem.AddCrystals(2);

        await MegaCrit.Sts2.Core.Commands.DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}


