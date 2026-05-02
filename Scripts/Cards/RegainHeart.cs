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
public class RegainHeart : YukiCardModel
{
    
    public RegainHeart() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None, true) { }

    public override string PortraitPath => "res://yuuki/images/cards/KORONA_FD_e01b.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("DrawCount", 3m),
        new IntVar("ExtraDraw", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        int baseDraw = (int)base.DynamicVars["DrawCount"].BaseValue;
        await CardPileCmd.Draw(choiceContext, baseDraw, base.Owner);

        
        var empathyTarget = base.CombatState.Enemies.FirstOrDefault(e => e.IsAlive && e.HasPower<EmpathyPower>());
        if (empathyTarget != null)
        {
            
            await PowerCmd.Remove<EmpathyPower>(empathyTarget);

            
            int extraDraw = (int)base.DynamicVars["ExtraDraw"].BaseValue;
            await CardPileCmd.Draw(choiceContext, extraDraw, base.Owner);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["ExtraDraw"].UpgradeValueBy(1m);
    }
}
