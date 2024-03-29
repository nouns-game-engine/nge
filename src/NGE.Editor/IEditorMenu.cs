﻿using Microsoft.Xna.Framework;

namespace NGE.Editor
{

    public interface IEditorMenu : IEditorEnabled
    {
        string Label { get; }
        void UpdateLayout(IEditingContext context, GameTime gameTime) { }
        void DrawLayout(IEditingContext context, GameTime gameTime);
    }
}