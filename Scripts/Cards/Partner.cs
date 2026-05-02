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
    
    public Partner() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesEmpathy => true;

    
    public override string PortraitPath => "res://yuuki/images/cards/RINNE_e10b.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Draw", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        int drawBase = (int)base.DynamicVars["Draw"].BaseValue;
        
        
        int empathyCount = base.CombatState.Enemies.Count(e => e.IsAlive && e.HasPower<EmpathyPower>());
        
        int totalDraw = drawBase + empathyCount;
        
        if (totalDraw > 0)
        {
            
            await CardPileCmd.Draw(choiceContext, totalDraw, base.Owner);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Draw"].UpgradeValueBy(1m);
    }
}
