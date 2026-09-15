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
        int crystals = (card.CombatState != null) ? yuuki.Scripts.YukiCrystalSystem.CurrentCrystals : 0;
        this.BaseValue = (decimal)(crystals + 2);
        base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
    }
}


