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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FFXI.XIACE {

    /// <summary>
    /// POL プロセス情報
    /// </summary>
    public class PolProcess {

        private Process _Process;
        private IntPtr _BaseAddress;

        public IntPtr Handle { get { return _Process.Handle; } }
        public IntPtr BaseAddress { get { return _BaseAddress; } }
        public string Title { get { return _Process.MainWindowTitle; } }
        public int Pid { get { return _Process.Id; } }

        public PolProcess(Process Proc) {
            _Process = Proc;
            foreach (ProcessModule m in Proc.Modules) {
                if (m.ModuleName == "FFXiMain.dll") {
                    _BaseAddress = m.BaseAddress;
                    break;
                }
            }
        }

        override public string ToString() {
            return Title;
        }
    }

    /// <summary>
    /// スタティッククラス
    /// </summary>
    public static class XIACE {
        /// <summary>
        /// Pol.EXEのプロセスリスト
        /// </summary>
        /// <returns></returns>
        public static PolProcess[] ListPolProcess() {
            Process[] proc = Process.GetProcessesByName("pol");
            PolProcess[] pol = new PolProcess[proc.Length];
            for (int i = 0; i < proc.Length; i++) {
                pol[i] = new PolProcess(proc[i]);
            }
            return pol;
        }
    }

    /// <summary>
    /// 実際にメモリにアクセスするスタティッククラス
    /// </summary>
    public static class MemoryProvider {

        /// <summary>
        /// ポインタ型をつかうunsafeメソッド : ポインタから配列に突っ込む(memcpy的な)ときにfixed ステートメントが必要
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="buffer"></param>
        /// <param name="nBufferSize"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        unsafe public extern static bool ReadProcessMemory(IntPtr handle, IntPtr address,
            void* buffer, uint nBufferSize, int* len);

        /// <summary>
        /// ポインタ型を使わないメソッド : Marshal.PtrTo*をつかってがんばれ
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="addr"></param>
        /// <param name="OutputBuffer"></param>
        /// <param name="nBufferSize"></param>
        /// <param name="lpNumberOfBytesRead"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public extern static bool ReadProcessMemorySafe(IntPtr handle, IntPtr addr, IntPtr OutputBuffer, UIntPtr nBufferSize, out UIntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        unsafe public extern static bool WriteProcessMemory(IntPtr handle, IntPtr address,
            void* buffer, uint nBufferSize, int* len);

        public static IntPtr ReadProcessMemorySafe(IntPtr Handle, IntPtr Address, uint nBytesToRead) {
            IntPtr Buffer = Marshal.AllocHGlobal((int) nBytesToRead);
            UIntPtr BytesRead = UIntPtr.Zero;
            UIntPtr BytesToRead = (UIntPtr) nBytesToRead;
            if (!ReadProcessMemorySafe(Handle, Address, Buffer, BytesToRead, out BytesRead)) {
                return IntPtr.Zero;
            }
            return Buffer;
        }

        public static string ReadMemoryString(IntPtr handle, IntPtr addr, uint size) {
            IntPtr Buffer = IntPtr.Zero;
            string str = string.Empty;
            Buffer = ReadProcessMemorySafe(handle, addr, size);
            try {
                str = Marshal.PtrToStringAnsi(Buffer, (int) size);
                str = str.Trim('\0');
            } finally {
                Marshal.FreeHGlobal(Buffer);
            }
            return str;
        }

        public static int ReadMemoryInt32(IntPtr handle, IntPtr addr) {
            IntPtr Buffer = IntPtr.Zero;
            int i = 0;
            Buffer = ReadProcessMemorySafe(handle, addr, 4);
            try {
                i = Marshal.ReadInt32(Buffer);
            } finally {
                Marshal.FreeHGlobal(Buffer);
            }
            return i;
        }

        public static short ReadMemoryInt16(IntPtr handle, IntPtr addr) {
            IntPtr Buffer = IntPtr.Zero;
            short i = 0;
            Buffer = ReadProcessMemorySafe(handle, addr, 2);
            try {
                i = Marshal.ReadInt16(Buffer);
            } finally {
                Marshal.FreeHGlobal(Buffer);
            }
            return i;
        }

        public static byte ReadMemoryByte(IntPtr handle, IntPtr addr) {
            IntPtr Buffer = IntPtr.Zero;
            byte ret;
            Buffer = ReadProcessMemorySafe(handle, addr, 1);
            try {
                ret = Marshal.ReadByte(Buffer);
            } finally {
                Marshal.FreeHGlobal(Buffer);
            }
            return ret;
        }

        unsafe public static void memcpy(byte* src, byte* dst, int len) {
            while (len-- > 0)
                *dst++ = *src++;
        }
    }
}
