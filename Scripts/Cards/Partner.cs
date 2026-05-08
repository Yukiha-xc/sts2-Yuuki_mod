using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Partner : YukiCardModel
{
    public Partner() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesEmpathy => true;

    public override string PortraitPath => "res://yuuki/images/cards/RINNE_e10b.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);

        
        int empathyCount = base.CombatState.Enemies.Count(e => e.IsAlive && e.HasPower<EmpathyPower>());
        
        if (empathyCount > 0)
        {
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(choiceContext, base.Owner.Creature, 
                (decimal)empathyCount, 
                base.Owner.Creature, 
                this
            );
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}

