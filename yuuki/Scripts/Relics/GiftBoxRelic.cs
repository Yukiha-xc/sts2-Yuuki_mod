using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class GiftBoxRelic : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/gift_box_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/gift_box_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/gift_box_relic.png";

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 9m, false);

        EnchantmentModel glam = ModelDb.Enchantment<Glam>();
        
        List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards
            .Where((CardModel c) => glam.CanEnchant(c))
            .ToList();
            
        CardModel? cardModel = (await CardSelectCmd.FromDeckForEnchantment(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), 
            cards: list.UnstableShuffle(base.Owner.RunState.Rng.Niche).ToList(), 
            enchantment: glam, 
            amount: 1)
        ).FirstOrDefault();
        
        if (cardModel != null)
        {
            CardCmd.Enchant<Glam>(cardModel, 1m);
            cardModel.AddKeyword(MegaCrit.Sts2.Core.Entities.Cards.CardKeyword.Innate);
            
            NCardEnchantVfx? nCardEnchantVfx = NCardEnchantVfx.Create(cardModel);
            Node? previewContainer = NRun.Instance?.GlobalUi.CardPreviewContainer;
            if (nCardEnchantVfx != null && previewContainer != null)
            {
                previewContainer.AddChildSafely(nCardEnchantVfx);
            }
        }
    }
}
