using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using yuuki.Scripts.Cards;
using yuuki.Scripts.Relics;

namespace yuuki.Scripts;


public class YukiCharacter : PlaceholderCharacterModel
{
    
    public override string PlaceholderID => "YUUKI";

    
    public override Color NameColor => new(0.95f, 0.95f, 1.0f);
    
    public override Color EnergyLabelOutlineColor => new(0.8f, 0.8f, 0.85f);

    
    public override CharacterGender Gender => CharacterGender.Feminine;

    
    public override int StartingHp => 70;

    
    public override string CustomVisualPath => "res://yuuki/scenes/yuki_visuals.tscn";
    
    
    public override string CustomIconTexturePath => "res://yuuki/images/ui/yuuki_icon.png";
    
    
    public override string CustomIconPath => "res://yuuki/scenes/character_icon.tscn";

    
    public override string CustomEnergyCounterPath => "res://yuuki/scenes/yuki_energy_counter.tscn";

    
    public override string CustomRestSiteAnimPath => "res://yuuki/scenes/yuki_rest_visuals.tscn";
    
    public override string CustomMerchantAnimPath => "res://yuuki/scenes/yuki_merchant_visuals.tscn";

    
    public override string CustomCharacterSelectBg => "res://yuuki/scenes/yuki_bg.tscn";
    
    
    public override string CustomCharacterSelectIconPath => "res://yuuki/images/ui/character_icon.png";
    
    
    public override string CustomCharacterSelectLockedIconPath => "res://yuuki/images/ui/character_icon.png";

    
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";

    
    public override string CustomArmPointingTexturePath => "res://yuuki/images/ui/pointing.png";
    public override string CustomArmRockTexturePath => "res://yuuki/images/ui/shitou.png";
    public override string CustomArmPaperTexturePath => "res://yuuki/images/ui/bu.png";
    public override string CustomArmScissorsTexturePath => "res://yuuki/images/ui/jiandao.png";

    
    public override string CustomMapMarkerPath => "res://yuuki/images/ui/snow_crystal.png";

    
    public override string CustomAttackSfx => "event:/sfx/characters/ironclad/ironclad_attack";
    public override string CustomCastSfx => "event:/sfx/characters/ironclad/ironclad_cast";
    public override string CustomDeathSfx => "event:/sfx/characters/ironclad/ironclad_die";
    
    private static readonly string[] SelectSounds = {
        "res://yuuki/audio/yuki_select.ogg",
        "res://yuuki/audio/001300540.ogg",
        "res://yuuki/audio/001300620.ogg"
    };
    
    public override string CharacterSelectSfx => SelectSounds[new Random().Next(SelectSounds.Length)];

    
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    
    public override CardPoolModel CardPool => ModelDb.CardPool<YukiPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<YukiRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<YukiPotionPool>();

    
    public override IEnumerable<CardModel> StartingDeck => new List<CardModel>
    {
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<FallingSnow>(),
        ModelDb.Card<SnowCrystalAttack>()
    };

    
    public override IReadOnlyList<RelicModel> StartingRelics => new List<RelicModel>
    {
        ModelDb.Relic<PureWhiteGift>()
    };

    public override float AttackAnimDelay => 0.1f;
    public override float CastAnimDelay => 0.1f;

    
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}
