namespace NGE.Editor
{
    public interface IEditorDropHandler : IEditorEnabled
    {
        bool Handle(IEditingContext context, params string[] fullPaths);
    }
}