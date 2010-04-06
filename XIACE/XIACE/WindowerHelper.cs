/* WindowerHelper wrapper class.
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
using System.Runtime.InteropServices;

namespace FFXI
{
    #region "Static Class"
    /// <summary>
    /// 
    /// </summary>
    public static class WindowerHelper
    {
        [DllImport("WindowerHelper.dll")]
        public static extern int CreateTextHelper(string name);

        [DllImport("WindowerHelper.dll")]
        public static extern void DeleteTextHelper(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHCreateTextObject(int helper, string name);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHDeleteTextObject(int helper, string name);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetText(int helper, string name, string text);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetVisibility(int helper, string name, bool visible);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetFont(int helper, string name, ref byte font, short height);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetColor(int helper, string name, byte a, byte r, byte g, byte b);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetLocation(int helper, string name, float x, float y);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetBold(int helper, string name, bool bold);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetItalic(int helper, string name, bool italic);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetBGColor(int helper, string name, byte a, byte r, byte g, byte b);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetBGBorderSize(int helper, string name, float pixels);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetBGVisibility(int helper, string name, bool visible);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHSetRightJustified(int helper, string name, bool justified);

        [DllImport("WindowerHelper.dll")]
        public static extern void CTHFlushCommands(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern int CreateKeyboardHelper(string name);

        [DllImport("WindowerHelper.dll")]
        public static extern void DeleteKeyboardHelper(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern void CKHSetKey(int helper, byte key, bool down);

        [DllImport("WindowerHelper.dll")]
        public static extern void CKHBlockInput(int helper, bool block);

        [DllImport("WindowerHelper.dll")]
        public static extern void CKHSendString(int helper, string data);

        [DllImport("WindowerHelper.dll")]
        public static extern int CreateConsoleHelper(string name);

        [DllImport("WindowerHelper.dll")]
        public static extern void DeleteConsoleHelper(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern bool CCHIsNewCommand(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern short CCHGetArgCount(int helper);

        [DllImport("WindowerHelper.dll")]
        public static extern void CCHGetArg(int helper, short index, byte[] buffer);

        #region "KeyCodes"
        public enum KeyCode
        {
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~~~~~~~These Here Are The Most Important FFXI Keys~~~~~~~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            EscapeKey = 1,

            EnterKey = 28,
            TabKey = 15,

            UpArrow = 200,
            LeftArrow = 203,
            RightArrow = 205,
            DownArrow = 208,
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~~~~~These Here Are The NumPad Keys On Your Keyboard~~~~~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            MainNumlockKey = 69,

            NP_Number0 = 82,
            NP_Number1 = 79,
            NP_Number2 = 80,
            NP_Number3 = 81,
            NP_Number4 = 75,
            NP_Number5 = 76,
            NP_Number6 = 77,
            NP_Number7 = 71,
            NP_Number8 = 72,
            NP_Number9 = 73,

            NP_Forwardslash = 181,
            NP_MultiplyKey = 55,
            NP_MinusKey = 74,
            NP_AdditionKey = 78,
            NP_EnterKey = 156,
            NP_PeriodKey = 83,
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~These Here Are The Letters From A to Z On Your Keyboard~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            LetterA = 30,
            LetterB = 48,
            LetterC = 46,
            LetterD = 32,
            LetterE = 18,
            LetterF = 33,
            LetterG = 34,
            LetterH = 35,
            LetterI = 23,
            LetterJ = 36,
            LetterK = 37,
            LetterL = 38,
            LetterM = 50,
            LetterN = 49,
            LetterO = 24,
            LetterP = 25,
            LetterQ = 16,
            LetterR = 19,
            LetterS = 31,
            LetterT = 20,
            LetterU = 22,
            LetterV = 47,
            LetterW = 17,
            LetterX = 45,
            LetterY = 21,
            LetterZ = 44,
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~These Here Are The Numbers From 0 to 9 On Your Keyboard~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Number1 = 2,
            Number2 = 3,
            Number3 = 4,
            Number4 = 5,
            Number5 = 6,
            Number6 = 7,
            Number7 = 8,
            Number8 = 9,
            Number9 = 10,
            Number0 = 11,
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~These Here Are The F Keys From F1 to F12 On Your Keyboard~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            F1Key = 59,
            F2Key = 60,
            F3Key = 61,
            F4Key = 62,
            F5Key = 63,
            F6Key = 64,
            F7Key = 65,
            F8Key = 66,
            F9Key = 67,
            F10Key = 68,
            F11Key = 87,
            F12Key = 88,
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~~These Here Are Ones That You Should Not Need But Here~~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //MinusKey = 12,
            //EqualsKey = 13,
            //BackspaceKey = 14,
            //LeftBracket = 26,
            //RightBracket = 27,
            LeftCtrlKey = 29,
            //Semicolon = 39,
            //Apostrophe = 40,
            //Accentgrave = 41,
            //LeftShift = 42,
            //Backslash = 43,
            //CommaKey = 51,
            //PeriodKey = 52,
            //ForwardslashKEy = 53,
            //RightShift = 54,
            //ScrollLock = 70,
            //LeftAltKey = 56,
            //Spacebar = 57,
            //CapsLock = 58,
            RightControlKey = 157,
            //RightAltKey = 184,
            //InsertKey = 210,
            //DeleteKey = 211,
            //LeftWindowKey = 219,
            //RightWindowKey = 220

            //Calculator = &HA1

            //MuteKey = &HA0
            //PlayNPauseKey = &HA2
            //StopMedia = &HA
            //VolumeDown = &HAE
            //VolumeUp = &HB0
            //NextMediaTrack = &HED
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        }
        #endregion
    }
    #endregion
}
