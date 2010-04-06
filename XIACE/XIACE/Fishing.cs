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

namespace FFXI.XIACE {

    internal struct FishingState {
        internal int rodPos;
        internal int unknown0;
        internal int unknown1;
        internal int unknown2;
        internal int unknown3;
        internal int unknownPtr;
        internal int fishPtr;
    }

    internal struct Fish {
        internal int MaxHP;
        internal int HP;
        internal int ID3;
        internal int unknown0;
        internal int ID1;
        internal int ID2;
        internal int ID4;
        internal int timeout;
        internal int unknown1;
        internal int unknown2;
        internal int rodPos;
    }

    public class Fishing : XIWindowerSubBase {

        private FishingState fishingState;
        private Fish fish;

        public Fishing(XIWindower windower)
            : base(windower) {
        }

        unsafe private void Read() {
            FishingState fs = new FishingState();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("FISH_INFO")), &fs, (uint) Marshal.SizeOf(fs), null);
            fishingState = fs;
            Fish f = new Fish();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (fs.fishPtr), &f, (uint) Marshal.SizeOf(f), null);
            fish = f;
        }

        public int GetFishMaxHP() {
            return fish.MaxHP;
        }

        public int GetFishHP() {
            Read();
            return fish.HP;
        }

        public int GetFishTimeout() {
            Read();
            return fish.timeout;
        }

        public int GetFishID1() {
            Read();
            return fish.ID1;
        }

        public int GetFishID2() {
            Read();
            return fish.ID2;
        }

        public int GetFishID3() {
            Read();
            return fish.ID3;
        }

        public int GetFishID4() {
            Read();
            return fish.ID4;
        }

        public int GetFishOnLineTime() {
            ///FIXME: still not implemented.
            return 0;
        }

        /// <summary>
        /// FIXME: untested.
        /// </summary>
        /// <returns></returns>
        public bool FishOnLine() {
            Read();
            if (fishingState.rodPos < 1)
                return false;
            else
                return true;
        }

        public eRodPosition GetRodposition() {
            Read();
            if (fishingState.rodPos == 1)
                return eRodPosition.Center;
            if (fishingState.rodPos == 2 && fish.rodPos == 0)
                return eRodPosition.Left;
            if (fishingState.rodPos == 2 && fish.rodPos == 1)
                return eRodPosition.Right;
            return eRodPosition.Error;
        }
    }
}
