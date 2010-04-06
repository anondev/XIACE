using System;
using System.Text;

namespace FFXI.XIACE {

    public class Menu : XIWindowerSubBase {

        private byte[] help = new byte[50];

        public Menu(XIWindower windower)
            : base(windower) {
        }

        unsafe private void ReadHelp() {
            int addr;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("MENU_HELP")), &addr, sizeof(int), null);
            fixed (byte* buf = help) {
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x123), buf, sizeof(byte) * 50, null);
            }
        }

        unsafe public byte GetIndex() {
            int addr;
            byte index;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("CURSOR_INFO")), &addr, sizeof(int), null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x4c), &index, sizeof(byte), null);
            return index;
        }

        public bool MenuIsOpen() {
            return false;
        }

        public string MenuName() {
            return null;
        }

        public string MenuSelection() {
            return null;
        }

        public string MenuHelp() {
            ReadHelp();
            return Encoding.GetEncoding("shift_jis").GetString(help);
        }
    }
}
