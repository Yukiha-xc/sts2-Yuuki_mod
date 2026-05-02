using BaseLib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;

namespace yuuki.Scripts.Cards;

public class CrystalDamageVar : DamageVar
{
    public CrystalDamageVar() : base(0, ValueProp.Move) { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        int crystals = yuuki.Scripts.YukiCrystalSystem.CurrentCrystals;
        this.BaseValue = (decimal)(crystals * 2);
        base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
    }
}

[Pool(typeof(yuuki.Scripts.YukiPool))]
public class SnowCrystalAttack : yuuki.Scripts.Cards.YukiCardModel, ITranscendenceCard
{
    public SnowCrystalAttack() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true) { }

    public override bool UsesSnowCrystals => true;

    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<WhiteOath>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CrystalDamageVar(),
        new DynamicVar("YukiConsume", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        DamageVar dVar = (DamageVar)base.DynamicVars["Damage"];
        decimal dmgValue = dVar.BaseValue;
        await DamageCmd.Attack(dmgValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        DynamicVar cVar = base.DynamicVars["YukiConsume"];
        int consumeAmount = (int)cVar.BaseValue;
        if (consumeAmount > 0)
        {
            yuuki.Scripts.YukiCrystalSystem.AddCrystals(-consumeAmount);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["YukiConsume"].UpgradeValueBy(-1m);
    }
}

