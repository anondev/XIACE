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
using System;
using System.Text;
using System.Threading;

namespace FFXI {

    public class Windower {

        // Private
        private int _ConsoleHelper;
        private int _TextHelper;
        private int _KeyboardHelper;

        // キーストロークのDOWN UPの間隔 (SendKey用)
        private int KeyPressDelay = 100;

        // 
        private int _Pid;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pid">対象プロセスID</param>
        public Windower(int pid) {
            _Pid = pid;
            _ConsoleHelper = WindowerHelper.CreateConsoleHelper("WindowerMMFConsoleHandler_" + pid.ToString());
            _TextHelper = WindowerHelper.CreateTextHelper("WindowerMMFTextHandler_" + pid.ToString());
            _KeyboardHelper = WindowerHelper.CreateKeyboardHelper("WindowerMMFKeyboardHandler_" + pid.ToString());
        }

        ~Windower() {
            Dispose();
        }

        public void Dispose() {
            WindowerHelper.DeleteConsoleHelper(_ConsoleHelper);
            WindowerHelper.DeleteTextHelper(_TextHelper);
            WindowerHelper.DeleteKeyboardHelper(_KeyboardHelper);
            _Pid = 0;
        }

        /// <summary>
        /// プロセスID
        /// </summary>
        public int Pid {
            get { return _Pid; }
        }

        /// <summary>
        /// テキストヘルパ
        /// </summary>
        public int TextHelper {
            get { return _TextHelper; }
        }

        /// <summary>
        /// 新規コマンドかどうか (バグってるの常にtrue (つまり使えない))
        /// </summary>
        public bool IsNewCommand {
            get { return WindowerHelper.CCHIsNewCommand(_ConsoleHelper); }
        }

        /// <summary>
        /// コマンド引数の数
        /// </summary>
        public short ArgCount {
            get { return WindowerHelper.CCHGetArgCount(_ConsoleHelper); }
        }

        /// <summary>
        /// FFXIチャットラインへテキストを送信 (コマンド可)
        /// </summary>
        /// <param name="text">テキスト</param>
        public void SendText(string text) {
            WindowerHelper.CKHSendString(_KeyboardHelper, text);
        }

        /// <summary>
        /// キー押下を送信
        /// </summary>
        /// <param name="code">KeyCode</param>
        /// <param name="down">true: down, false: up</param>
        public void KeyDown(WindowerHelper.KeyCode code, bool down) {
            WindowerHelper.CKHSetKey(_KeyboardHelper, Convert.ToByte(code), down);
        }

        /// <summary>
        /// キーストロークを送信
        /// </summary>
        /// <param name="code">キーコード</param>
        public void SendKey(WindowerHelper.KeyCode code) {
            WindowerHelper.CKHSetKey(_KeyboardHelper, Convert.ToByte(code), true);
            Thread.Sleep(KeyPressDelay);
            WindowerHelper.CKHSetKey(_KeyboardHelper, Convert.ToByte(code), false);
        }

        /// <summary>
        /// コンソールコマンドの文字列を取得
        /// </summary>
        /// <param name="index">argc index</param>
        /// <returns>文字列</returns>
        public string ConsoleGetArg(short index) {
            byte[] buffer = new byte[256]; // いまのところ256バイトのみ
            // text = String.Format("{0:255}", " ");
            WindowerHelper.CCHGetArg(_ConsoleHelper, index, buffer);
            var len = 0;
            for (var i = 0; i < buffer.Length; i++) {
                if (buffer[i] == 0) {
                    len = i;
                    break;
                }
            }
            return Encoding.GetEncoding("shift_jis").GetString(buffer, 0, len);
        }

        /// <summary>
        /// コンソールコマンドの引数の数を取得
        /// </summary>
        /// <returns></returns>
        public int ConsoleGetArgCount() {
            return WindowerHelper.CCHGetArgCount(_ConsoleHelper);
        }

        /// <summary>
        /// テキストオブジェクトを作成
        /// </summary>
        /// <param name="name">name of TEXT Object</param>
        /// <returns>id of TEXT Object</returns>
        public TextObject CreateTextObject(string name) {
            return new TextObject(this, name);
        }
    }

    /// <summary>
    /// テキストオブジェクトクラス
    /// </summary>
    public class TextObject {
        private Windower _Parent;
        private string _Name;

        public Windower Parent { get { return _Parent; } }
        public string Name { get { return _Name; } }

        public TextObject(Windower w, string name) {
            _Parent = w;
            _Name = name;
            WindowerHelper.CTHCreateTextObject(w.TextHelper, name);
        }

        public void SetText(string text) {
            WindowerHelper.CTHSetText(_Parent.TextHelper, _Name, text);
        }

        public void SetLocation(float x, float y) {
            WindowerHelper.CTHSetLocation(_Parent.TextHelper, _Name, x, y);
        }

        public void SetBold(bool bold) {
            WindowerHelper.CTHSetBold(_Parent.TextHelper, _Name, bold);
        }

        public void SetItalic(bool italic) {
            WindowerHelper.CTHSetItalic(_Parent.TextHelper, _Name, italic);
        }

        public void SetBGColor(byte Alpha, byte Red, byte Green, byte Blue) {
            WindowerHelper.CTHSetBGColor(_Parent.TextHelper, _Name, Alpha, Red, Green, Blue);
        }

        public void SetBGVisibilitiy(bool visible) {
            WindowerHelper.CTHSetBGVisibility(_Parent.TextHelper, _Name, visible);
        }

        public void SetFontColor(byte Alpha, byte Red, byte Green, byte Blue) {
            WindowerHelper.CTHSetColor(_Parent.TextHelper, _Name, Alpha, Red, Green, Blue);
        }

        public void SetVisibility(bool visible) {
            WindowerHelper.CTHSetVisibility(_Parent.TextHelper, _Name, visible);
        }

        ~TextObject() {
            Dispose();
        }

        public void Dispose() {
            WindowerHelper.CTHDeleteTextObject(_Parent.TextHelper, _Name);
            Flush();
        }

        public void Flush() {
            WindowerHelper.CTHFlushCommands(_Parent.TextHelper);
        }
    }
}
