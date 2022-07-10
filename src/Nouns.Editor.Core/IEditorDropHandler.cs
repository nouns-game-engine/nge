namespace Nouns.Editor;

public interface IEditorDropHandler : IEditorEnabled
{
    bool Handle(IEditorContext context, params string[] files);
}