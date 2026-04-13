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


        public const byte VK_CONTROL = 0x11; // Ctrl
        public const byte VK_MENU = 0x12; // Alt
        public const byte VK_SHIFT = 0x10; // Shift

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
        struct INPUT
        {
            public int type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
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
                Thread.Sleep(100);
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
            logger.LogInformation($"Attempting to send key: {keyText}");

            // Split keyText into parts
            var parts = keyText.ToUpper().Split('+');

            var modifiers = new List<ushort>();
            ushort mainKey = 0;

            foreach (var part in parts)
            {
                var lookup = part.Replace(" ", "");

                if (!CubaseKeyMap.Map.TryGetValue(lookup, out byte vk))
                {
                    errHandler($"Could not find a keyboard mapping for {part}");
                    return false;
                }

                // Add modifiers
                if (vk == VK_CONTROL || vk == VK_MENU || vk == VK_SHIFT)
                    modifiers.Add(vk);
                else
                    mainKey = vk; // last non-modifier = main key
            }

            if (mainKey == 0)
            {
                errHandler("No main key specified");
                return false;
            }

            // Order modifiers: Ctrl, Alt, Shift
            var orderedMods = new List<ushort>();
            if (modifiers.Contains(VK_CONTROL)) orderedMods.Add(VK_CONTROL);
            if (modifiers.Contains(VK_MENU)) orderedMods.Add(VK_MENU);
            if (modifiers.Contains(VK_SHIFT)) orderedMods.Add(VK_SHIFT);

            try
            {
                // --- PHASE 1: PRESS MODIFIERS ---
                var modInputs = new List<INPUT>();
                foreach (var mod in orderedMods)
                    modInputs.Add(CreateKeyInput(mod, false));

                uint rc = 0;

                if (modInputs.Count > 0)
                    rc = SendInput((uint)modInputs.Count, modInputs.ToArray(), Marshal.SizeOf<INPUT>());

                if (!CheckForError(rc))
                    return false;

                Thread.Sleep(50); // let Cubase register modifiers

                // --- PHASE 2: PRESS & RELEASE MAIN KEY ---
                var keyInputs = new INPUT[]
                {
                   CreateKeyInput(mainKey, false),
                   CreateKeyInput(mainKey, true)
                };
                rc = SendInput((uint)keyInputs.Length, keyInputs, Marshal.SizeOf<INPUT>());

                if (!CheckForError(rc))
                    return false;

                Thread.Sleep(50); // optional small pause

                // --- PHASE 3: RELEASE MODIFIERS (reverse order) ---
                var releaseMods = new List<INPUT>();
                for (int i = orderedMods.Count - 1; i >= 0; i--)
                    releaseMods.Add(CreateKeyInput(orderedMods[i], true));

                if (releaseMods.Count > 0)
                    SendInput((uint)releaseMods.Count, releaseMods.ToArray(), Marshal.SizeOf<INPUT>());

                bool CheckForError(uint result)
                {
                    if (result == 0)
                    {
                        var err = Marshal.GetLastWin32Error();
                        errHandler($"SendInput failed: {err} - {GetWin32ErrorMessage(err)}");
                        return false;
                    }
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                errHandler($"Exception in SendKey: {ex.Message}");
                return false;
            }
        }

        private INPUT CreateKeyInput(ushort vk, bool keyUp)
        {
            return new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = vk,
                        wScan = 0,
                        dwFlags = keyUp ? KEYEVENTF_KEYUP : 0,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };
        }




    }
}