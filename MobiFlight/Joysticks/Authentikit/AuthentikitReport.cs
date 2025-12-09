using SharpDX.DirectInput;
using System;

namespace MobiFlight.Joysticks
{
    internal class AuthentikitReport
    {
        private byte[] LastInputBufferState = new byte[5];

        public void CopyFromInputBuffer(byte[] inputBuffer)
        {
            if (inputBuffer == null || inputBuffer.Length < LastInputBufferState.Length)
            {
                throw new ArgumentException($"Invalid input buffer length. Expected {LastInputBufferState.Length}, got {inputBuffer?.Length ?? 0}");
            }
            LastInputBufferState = (byte[])inputBuffer.Clone();
        }

        public AuthentikitReport Parse(byte[] inputBuffer)
        {
            var result = new AuthentikitReport();
            result.CopyFromInputBuffer(inputBuffer);

            return result;
        }

        public JoystickState ToJoystickState()
        {
            JoystickState state = new JoystickState();

            // Buttons
            for (int i = 0; i < 12; i++)
            {
                int byteIndex = (i / 8);
                int bitIndex = i % 8;
                bool isPressed = (LastInputBufferState[byteIndex] & (1 << bitIndex)) != 0;
                state.Buttons[i] = isPressed;
            }

            return state;
        }
    }
}
