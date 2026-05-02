using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class NoMoreGoodbyes : YukiCardModel
{
    
    public NoMoreGoodbyes() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    public override string PortraitPath => "res://yuuki/images/cards/YUKI_e10b2.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<NoMoreGoodbyesPower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}
