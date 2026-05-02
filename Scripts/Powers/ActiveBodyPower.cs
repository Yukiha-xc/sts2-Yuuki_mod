using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Cards;

namespace yuuki.Scripts.Powers;
public class ActiveBodyPower : TemporaryStrengthPower
{
    
public override AbstractModel OriginModel => ModelDb.Card<ActiveBody>();
}

