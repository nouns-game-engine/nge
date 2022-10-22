using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace NGE.Core
{
    public static class Input
    {
        public static MouseState mouseState;
        public static MouseState lastMouseState;

        public static KeyboardState keyboardState;
        public static KeyboardState lastKeyboardState;

        private static readonly GamePadState[] gamePadStates = new GamePadState[4];
        private static readonly GamePadState[] lastGamePadStates = new GamePadState[4];

        public static bool IsActive { get; private set; }

        public static void Update(bool isActive)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            UpdateScrollWheel();

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            Array.Copy(gamePadStates, lastGamePadStates, gamePadStates.Length);

            for (var i = 0; i < 4; i++)
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);

            IsActive = isActive;
        }


        #region Key Presses

        public static bool KeyWentDown(Keys key)
        {
            return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }

        public static bool KeyWentUp(Keys key)
        {
            return lastKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
        }

        #endregion


        #region Keyboard State

        public static bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public static bool Shift
        {
            get { return keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift); }
        }

        public static bool Control
        {
            get { return keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl); }
        }

        public static bool Alt
        {
            get { return keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt); }
        }

        #endregion


        #region Mouse State

        public static bool MouseWentDown => LeftMouseWentDown || MiddleMouseWentDown || RightMouseWentDown;

        public static bool MouseWentUp => LeftMouseWentUp || MiddleMouseWentUp || RightMouseWentUp;

        public static bool MouseIsDown => LeftMouseIsDown || MiddleMouseIsDown || RightMouseIsDown;

        public static bool MouseIsUp => LeftMouseIsUp || MiddleMouseIsUp || RightMouseIsUp;

        public static bool LeftMouseWentDown => IsActive && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;

        public static bool LeftMouseWentUp => IsActive && lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;

        public static bool LeftMouseIsDown => IsActive && mouseState.LeftButton == ButtonState.Pressed;

        public static bool LeftMouseIsUp => !IsActive || mouseState.LeftButton == ButtonState.Released;

        public static bool RightMouseWentDown => IsActive && lastMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed;

        public static bool RightMouseWentUp => IsActive && lastMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released;

        public static bool RightMouseIsDown => IsActive && mouseState.RightButton == ButtonState.Pressed;

        public static bool RightMouseIsUp => !IsActive || mouseState.RightButton == ButtonState.Released;

        public static bool MiddleMouseWentDown => IsActive && lastMouseState.MiddleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed;

        public static bool MiddleMouseWentUp => IsActive && lastMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released;

        public static bool MiddleMouseIsDown => IsActive && mouseState.MiddleButton == ButtonState.Pressed;

        public static bool MiddleMouseIsUp => !IsActive || mouseState.MiddleButton == ButtonState.Released;

        public static Point MousePosition => new(mouseState.X, mouseState.Y);

        public static Point LastMousePosition => new(lastMouseState.X, lastMouseState.Y);

        public static Point LeftMouseDrag =>
            IsActive && mouseState.LeftButton == ButtonState.Pressed &&
            lastMouseState.LeftButton == ButtonState.Pressed
                ? new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y)
                : new Point();

        public static Point MiddleMouseDrag =>
            IsActive && mouseState.MiddleButton == ButtonState.Pressed &&
            lastMouseState.MiddleButton == ButtonState.Pressed
                ? new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y)
                : new Point();

        public static Point RightMouseDrag =>
            IsActive && mouseState.RightButton == ButtonState.Pressed &&
            lastMouseState.RightButton == ButtonState.Pressed
                ? new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y)
                : new Point();

        #endregion


        #region Mouse Scroll

        public static int scrollWheelAccumulate;
        public static bool ScrollWheelClickUp { get; private set; }
        public static bool ScrollWheelClickDown { get; private set; }
        public static int ScrollWheelClick { get; private set; }

        private static void UpdateScrollWheel()
        {
            ScrollWheelClickUp = ScrollWheelClickDown = false;
            ScrollWheelClick = 0;

            scrollWheelAccumulate += (mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue);

            if (scrollWheelAccumulate >= 120)
            {
                ScrollWheelClickUp = true;
                ScrollWheelClick = 1;
                scrollWheelAccumulate -= 120;
            }
            if (scrollWheelAccumulate <= -120)
            {
                ScrollWheelClickDown = true;
                ScrollWheelClick = -1;
                scrollWheelAccumulate += 120;
            }
        }

        #endregion


        #region Keyboard Movement

        public static Point SimpleKeyboardMovement
        {
            get
            {
                Point move = Point.Zero;
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                    move.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    move.X += 1;
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    move.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                    move.Y += 1;
                return move;
            }
        }

        #endregion


        #region Gamepad

        public static GamePadState GamePadState(int playerIndex) { return gamePadStates[playerIndex]; }

        public static bool GamePadButtonWentDown(int playerIndex, Buttons button) => gamePadStates[playerIndex].IsButtonDown(button) && !lastGamePadStates[playerIndex].IsButtonDown(button);

        public static bool GamePadButtonWentUp(int playerIndex, Buttons button) => !gamePadStates[playerIndex].IsButtonDown(button) && lastGamePadStates[playerIndex].IsButtonDown(button);

        public static bool GamePadButtonIsDown(int playerIndex, Buttons button) => gamePadStates[playerIndex].IsButtonDown(button);

        public static bool GamePadButtonIsUp(int playerIndex, Buttons button) => !gamePadStates[playerIndex].IsButtonUp(button);

        public static void GamePadRumble(int playerIndex, float leftMotor, float rightMotor)
        {
            GamePad.SetVibration((PlayerIndex) playerIndex, leftMotor, rightMotor);
        }

        #endregion
    }
}