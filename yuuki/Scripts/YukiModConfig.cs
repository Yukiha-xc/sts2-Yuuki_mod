using BaseLib.Config;

namespace yuuki.Scripts;

[ConfigHoverTipsByDefault]
public sealed class YukiModConfig : SimpleModConfig
{
    [ConfigSection("施工中")]
    [ConfigHoverTip]
    public static bool UnderConstruction { get; set; } = true;
}
