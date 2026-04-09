using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Cubase.Macro.Services.Window;

namespace Cubase.Macro.Services.Keyboard
{
    public class KeyboardService : IKeyboardService
    {
        private readonly ILogger<KeyboardService> logger;
        private readonly IWindowService windowService;

        public KeyboardService(ILogger<KeyboardService> logger, IWindowService windowService)
        {
            this.logger = logger;
            this.windowService = windowService;
        }

        #region Native

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_SCANCODE = 0x0008;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(
                 uint dwFlags,
                 IntPtr lpSource,
                 uint dwMessageId,
                 uint dwLanguageId,
                 System.Text.StringBuilder lpBuffer,
                 uint nSize,
                 IntPtr Arguments);

        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private string GetWin32ErrorMessage(int errorCode)
        {
            var sb = new System.Text.StringBuilder(512);
            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)errorCode, 0, sb, (uint)sb.Capacity, IntPtr.Zero);
            return sb.ToString();
        }

        #endregion

        public bool SendKeyToCubase(string keyText, Action<string> errHandler)
        {
            if (windowService.BringCubaseToFront())
            {
                return SendKey(keyText, errHandler);
            }
            else
            {
                errHandler($"Cannot send key {keyText} to Cubase. Is it running?");
                return false;
            }
        }

        public bool SendKey(string keyText, Action<string> errHandler)
        {
            this.logger.LogInformation($"Attempting to send key: {keyText}");
            // Split keyText into parts (modifiers + main key)
            var parts = keyText.ToUpper().Split('+');
            var modifiers = new List<byte>();
            byte key = 0;

            foreach (var part in parts)
            {
                // Normalize: remove spaces and handle Cubase naming quirks
                var lookup = part.Replace(" ", "");

                // Lookup in CubaseKeyMap
                if (CubaseKeyMap.Map.TryGetValue(lookup, out byte vk))
                {
                    if (part == parts[^1])
                        key = vk; // last part = main key
                    else
                        modifiers.Add(vk); // treat as modifier
                }
                else
                {
                    this.logger.LogWarning($"Could not find Could not find a keyboard mapping for {part}");
                    errHandler($"Could not find a keyboard mapping for {part}");
                    return false;
                }
            }

            // Press modifiers
            foreach (var mod in modifiers)
            {
                this.logger.LogInformation($"Sending {mod}");
                keybd_event(mod, 0, 0, UIntPtr.Zero);
            }

            // Press main key
            this.logger.LogInformation($"Sending main key {key}");
            keybd_event(key, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50); // simulate key press
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            // Release modifiers
            foreach (var mod in modifiers)
                keybd_event(mod, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            return true;
        }
    }
}