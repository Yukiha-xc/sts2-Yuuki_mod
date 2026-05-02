using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PureWhiteBride : YukiCardModel
{
    
    public PureWhiteBride() : base(2, CardType.Skill, CardRarity.Ancient, TargetType.AllEnemies, true) { }

    public override bool UsesEmpathy => true;

    
    public override string PortraitPath => "res://yuuki/images/cards/xiangu2.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        foreach (var enemy in base.CombatState.Enemies)
        {
            if (enemy.IsAlive)
            {
                await PowerCmd.Apply<EmpathyPower>(enemy, 1m, base.Owner.Creature, this);
            }
        }

        
        
        var voids = GetVoids(base.Owner).ToList();
        foreach (var v in voids)
        {
            await CardCmd.Exhaust(choiceContext, v);
        }

        await Cmd.Wait(0.25f);
    }

    private IEnumerable<CardModel> GetVoids(Player owner)
    {
        
        return owner.PlayerCombatState.AllCards.Where(c => c is MegaCrit.Sts2.Core.Models.Cards.Void && c.Pile != null && c.Pile.Type != PileType.Exhaust);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}
