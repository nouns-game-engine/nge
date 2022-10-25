using Microsoft.Xna.Framework.Input;

namespace NGE.Engine.InputManagement;

public struct KeyMapping<TPlayerButton> where TPlayerButton : Enum
{
    public TPlayerButton Key { get; set; }
    public Keys Value { get; set; }
}