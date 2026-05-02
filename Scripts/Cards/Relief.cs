using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Relief : YukiCardModel
{
    public Relief() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    public override bool GainsBlock => true;
    public override bool UsesEmpathy => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ReliefBlockVar(9m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(2m);
    }

    public class ReliefBlockVar : BlockVar
    {
        public ReliefBlockVar(decimal baseVal) : base(baseVal, ValueProp.Move) { }

        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
        {
            decimal originalBase = card.IsUpgraded ? 11m : 9m;
            bool hasEmpathy = false;
            if (card.CombatState != null)
            {
                hasEmpathy = card.CombatState.Enemies.Any(e => e.IsAlive && e.HasPower<EmpathyPower>());
            }
            decimal targetBase = hasEmpathy ? originalBase + 4m : originalBase;
            this.BaseValue = targetBase;
            base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
        }
    }
}

