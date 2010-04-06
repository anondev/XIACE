/* XIACE - Memory Access Provider for FFXI.
 * Copyright (C) 2009 FFXI RCM Project <ff11rcm@gmail.com>
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FFXI.XIACE {

    public class Inventory : XIWindowerSubBase {

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe public struct InventoryItem {
            public ushort id;      //2
            public ushort order;   //4
            public uint count;   //8
            public uint flag;    //12
            public uint _unknown;   //16
            public ushort extraCount; // 18
        }

        public Inventory(XIWindower windower)
            : base(windower) {
        }

        /// <summary>
        /// 装備品のカバンの中での位置
        /// </summary>
        /// <param name="slot">装備スロット</param>
        /// <returns>位置</returns>
        unsafe private int GetEquipItemPos(eEquipSlot slot) {
            int pos;
            int off = (int) Offset.Get("EQUIP_INFO") + ((int) slot * 8) + 4;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + off), &pos, 4, null);
            return (pos - 1);
        }

        /// <summary>
        /// アイテム情報の取得
        /// </summary>
        /// <param name="index">位置</param>
        /// <param name="offset">INVENTORY_INFO|SAFEBOX_INFO|STORAGE_INFO|LOCKER_INFO</param>
        /// <returns>InventoryItem 構造体</returns>
        unsafe private InventoryItem _GetInventoryItem(short index, Offset offset) {
            int off = (int) offset + index * 0x2c;
            /* short id;
            short order;
            int count;
            int extra;
            int flag;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr)((int)windower.pol.BaseAddress + off), &id, 2, null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr)((int)windower.pol.BaseAddress + off + 2), &order, 2, null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr)((int)windower.pol.BaseAddress + off + 4), &count, 4, null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr)((int)windower.pol.BaseAddress + off + 8), &flag, 4, null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr)((int)windower.pol.BaseAddress + off + 16), &extra, 4, null);
             */
            InventoryItem item = new InventoryItem();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + off), &item, 18, null);

            return item;
        }

        /// <summary>
        /// ItemID -> Name をメモリから読む
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        unsafe public string GetItemNameById(ushort id) {
            byte[] name = new byte[32];
            short sid;
            int i = 0;
            while (true) {
                // ここの構造体よくわかんないけど1個328バイト,
                // 0バイト目にItemID
                // 104バイト目にアイテム名
                int off = (int) Offset.Get("ITEM_INFO") + (i++ * 328);
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + off), &sid, 2, null);
                if (sid == 0)
                    break;
                if (sid == id) {
                    fixed (byte* buf = name) {
                        MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + off + 104), buf, 32, null);
                    }
                    return Encoding.GetEncoding("shift_jis").GetString(name).Trim('\0');
                }
            }
            return "";
        }

        /// <summary>
        /// カバンアイテム情報取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItem(short index) {
            return _GetInventoryItem(index, Offset.Get("INVENTORY_INFO"));
        }

        /// <summary>
        /// 金庫アイテム情報取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetSafeboxItem(short index) {
            return _GetInventoryItem(index, Offset.Get("SAFEBOX_INFO"));
        }

        /// <summary>
        /// 収納アイテム取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetStorageItem(short index) {
            return _GetInventoryItem(index, Offset.Get("STORAGE_INFO"));
        }

        /// <summary>
        /// ロッカーアイテム取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetLockerItem(short index) {
            return _GetInventoryItem(index, Offset.Get("LOCKER_INFO"));
        }

        /// <summary>
        /// サッチェルアイテム取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItem GetSatchelItem(short index) {
            return _GetInventoryItem(index, Offset.Get("SATCHEL_INFO"));
        }

        /// <summary>
        /// 装備アイテム名取得
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public string GetEquippedItemName(eEquipSlot slot) {
            int pos = GetEquipItemPos(slot);
            if (pos < 0)
                return "";
            InventoryItem item = _GetInventoryItem((short) pos, Offset.Get("INVENTORY_INFO"));
            if (item.id == 0)
                return "";
            return GetItemNameById(item.id);
        }

        /// <summary>
        /// 装備してるアイテムの数取得 : 事実上 ammo にしか意味なし
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public byte GetEquippedItemCount(eEquipSlot slot) {
            int pos = GetEquipItemPos(slot);
            if ((pos) < 0)
                return (byte) 0;
            InventoryItem item = _GetInventoryItem((short) pos, Offset.Get("INVENTORY_INFO"));
            if (item.id == 0)
                return (byte) 0;
            return (byte) item.count;
        }

        /// <summary>
        /// カバンアイテム数の取得
        /// </summary>
        /// <returns></returns>
        unsafe public byte GetInventoryCount() {
            int ptr;
            byte count;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("INVENTORY_COUNT")), &ptr, 4, null);
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (ptr + 0x24), &count, 1, null);
            return (byte) (count - 1);
        }

        unsafe private int GetMaxCount(int offset) {
            byte count;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) offset, &count, 1, null);
            return (int) (count - 1);
        }

        /// <summary>
        /// カバンアイテム最大数の取得
        /// </summary>
        /// <returns></returns>
        public int GetInventoryMax() {
            return GetMaxCount((int) windower.pol.BaseAddress + (int) Offset.Get("INVENTORY_MAX"));
        }
        /// <summary>
        /// 金庫最大
        /// </summary>
        /// <returns></returns>
        public int GetSafeboxMax() {
            return GetMaxCount((int) windower.pol.BaseAddress + (int) Offset.Get("INVENTORY_MAX") + 1);
        }
        /// <summary>
        /// 収納最大
        /// </summary>
        /// <returns></returns>
        public int GetStorageMax() {
            return GetMaxCount((int) windower.pol.BaseAddress + (int) Offset.Get("INVENTORY_MAX") + 2);
        }
        /// <summary>
        /// ロッカー最大
        /// </summary>
        /// <returns></returns>
        public int GetLockerMax() {
            return GetMaxCount((int) windower.pol.BaseAddress + (int) Offset.Get("INVENTORY_MAX") + 4);
        }

        /// <summary>
        /// サッチェル最大
        /// </summary>
        /// <returns></returns>
        public int GetSatchelMax() {
            return GetMaxCount((int) windower.pol.BaseAddress + (int) Offset.Get("INVENTORY_MAX") + 5);
        }

        /// <summary>
        /// カバンにある特定アイテムの数の取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint GetItemCountByIndex(short index) {
            InventoryItem item = _GetInventoryItem(index, Offset.Get("INVENTORY_INFO"));
            return item.count;
        }

        /// <summary>
        /// カバンにある特定アイテムのExtraカウント(WSPointなど)取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetExtraCountByIndex(short index) {
            InventoryItem item = _GetInventoryItem(index, Offset.Get("INVENTORY_INFO"));
            return item.extraCount;
        }

        unsafe public int GetGil() {
            int value;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("GIL_AMOUNT")), &value, 4, null);
            return value;
        }
    }
}
