using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class WhiteEternity : YukiCardModel
{
    
    public WhiteEternity() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<WhiteEternityPower>(
            base.Owner.Creature, 
            1m, 
            base.Owner.Creature, 
            this
        );

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}
