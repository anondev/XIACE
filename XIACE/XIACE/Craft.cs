using System;
using System.Runtime.InteropServices;

namespace FFXI.XIACE {

    public class Craft : XIWindowerSubBase {

        private ushort[] id = new ushort[9];
        private byte[] count = new byte[9];
        private byte[] order = new byte[9];

        public enum eCategory {
            Woodworking = 0,
            Smithing = 1,
            Goldsmithing = 2,
            Clothcraft = 3,
            Leathercraft = 4,
            Bonecrafting = 5,
            Alchemy = 6,
            Cooking = 7
        }

        [StructLayout(LayoutKind.Sequential)]
        public class RecipeItemSet : ICloneable {
            public int category;
            public string name;
            public RecipeItem[] item;
            public RecipeItemSet() {
                item = new Craft.RecipeItem[9];
                for (int i = 0; i < this.item.Length; i++) {
                    item[i] = new Craft.RecipeItem();
                }
            }
            override public string ToString() {
                return name;
            }
            public object Clone() {
                RecipeItemSet itemSet = new RecipeItemSet();
                itemSet.category = this.category;
                itemSet.name = this.name;
                itemSet.item = (RecipeItem[]) this.item.Clone();
                return itemSet;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class RecipeItem : ICloneable {
            public ushort id;
            public uint count;
            public string name;
            [System.Xml.Serialization.XmlIgnoreAttribute]
            public ushort order;
            [System.Xml.Serialization.XmlIgnoreAttribute]
            public bool check = false;
            public override string ToString() {
                return name;
            }
            public object Clone() {
                RecipeItem item = new RecipeItem();
                item.id = this.id;
                item.count = this.count;
                item.name = this.name;
                item.check = this.check;
                return item;
            }
        }

        public Craft(XIWindower windower)
            : base(windower) {
        }

        unsafe public void ReadOrder() {
            int addr;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("ORDER_INFO")), &addr, sizeof(int), null);
            fixed (ushort* buf = id) {
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x2a), buf, sizeof(ushort) * 9, null);
            }
            fixed (byte* buf = count) {
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x40), buf, sizeof(byte) * 9, null);
            }
            fixed (byte* buf = order) {
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x14), buf, sizeof(byte) * 9, null);
            }
        }

        unsafe public void WriteOrder(RecipeItem[] item, int startIndex) {
            if (item == null || ((item.Length - startIndex) < 8))
                return;
            for (int i = 0; i < 8; i++) {
                id[i] = (ushort) item[startIndex + i].id;
                count[i] = (byte) item[startIndex + i].count;
                order[i] = (byte) item[startIndex + i].order;
            }
            int addr;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("ORDER_INFO")), &addr, sizeof(int), null);
            fixed (ushort* buf = id) {
                MemoryProvider.WriteProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x2a + sizeof(ushort)), buf, sizeof(ushort) * 8, null);
            }
            fixed (byte* buf = count) {
                MemoryProvider.WriteProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x40), buf, sizeof(byte) * 8, null);
            }
            fixed (byte* buf = order) {
                MemoryProvider.WriteProcessMemory(windower.pol.Handle, (IntPtr) (addr + 0x14 + sizeof(byte)), buf, sizeof(byte) * 8, null);
            }
        }

        public RecipeItemSet GetRecipeItemSet() {
            ReadOrder();
            RecipeItemSet set = new RecipeItemSet();
            for (int i = 0; i < 9; i++) {
                set.item[i].id = id[i];
                set.item[i].count = count[i];
                set.item[i].order = order[i];
                //if (set.item[i].id != 0)
                //    set.item[i].name = windower.Inventory.GetItemNameById(id[i]);
            }
            if (set.item[0].id != 0)
                set.item[0].count = 1;
            return set;
        }
    }
}
