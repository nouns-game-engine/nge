using Microsoft.Xna.Framework.Input;

namespace NGE.Engine.InputManagement;

public struct ButtonMapping<TPlayerButton> where TPlayerButton : Enum
{
    public TPlayerButton Key { get; set; }
    public Buttons Value { get; set; }
}