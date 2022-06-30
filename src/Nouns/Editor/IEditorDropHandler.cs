namespace Nouns.Editor;

public interface IEditorDropHandler : IEditorEnabled
{
    bool Handle(params string[] files);
}