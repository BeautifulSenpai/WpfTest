using System;
using System.Runtime.InteropServices;

namespace WpfApp1.Classes
{
    public class KeyboardLockManager
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private const int VK_LWIN = 0x5B; // Левая клавиша Win
        private const int VK_RWIN = 0x5C; // Правая клавиша Win
        private const int VK_LALT = 0xA4; // Левая клавиша ALT
        private const int VK_RALT = 0xA5; // Правая клавиша ALT
        private const int VK_LCTRL = 0xA2; // Левая клавиша CTRL
        private const int VK_RCTRL = 0xA3; // Правая клавиша CTRL
        private const int VK_ESC = 0x1B; // Клавиша ESC
        private const int VK_DELETE = 0x2E; // Клавиша Delete на числовой клавиатуре (numpad)
        private const int VK_RETURN = 0x0D; // Клавиша Enter на числовой клавиатуре (numpad)
        private const int VK_SPACE = 0x20; // Клавиша Space (пробел)

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private bool isKeyboardLocked = false;

        public void LockKeyboard()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
            isKeyboardLocked = true;
        }

        public void UnlockKeyboard()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                isKeyboardLocked = false;
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (isKeyboardLocked && (vkCode == VK_LWIN || vkCode == VK_RWIN || vkCode == VK_LALT || vkCode == VK_RALT || vkCode == VK_LCTRL || vkCode == VK_RCTRL || vkCode == VK_ESC || vkCode == VK_DELETE || vkCode == VK_RETURN || vkCode == VK_SPACE))
                {
                    return (IntPtr)1;
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}