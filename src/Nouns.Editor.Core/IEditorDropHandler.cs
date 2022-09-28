namespace Nouns.Editor
{
    public interface IEditorDropHandler : IEditorEnabled
    {
        bool Handle(IEditingContext context, params string[] fullPaths);
    }
}