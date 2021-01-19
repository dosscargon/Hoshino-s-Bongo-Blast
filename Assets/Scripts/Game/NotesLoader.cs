using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts {
    public class NotesLoader {
        /// <summary>
        /// ノーツを配置するLaneManager
        /// </summary>
        private LaneController owner;
        private StringReader reader;

        public NotesLoader(TextAsset txt, LaneController owner) {
            reader = new StringReader(txt.text);
            this.owner = owner;
        }

        /// <summary>
        /// ファイルからノーツを読み込む
        /// </summary>
        public void Load() {
            string line;
            const string settingPattern = @".+?=.*?";
            float measureCountL;
            for (measureCountL = 0; (line = reader.ReadLine()) != null; ) {
                line = RemoveWaste(line);

                //行の種類を識別
                if (Regex.IsMatch(line, settingPattern)) {
                    Setting(line);
                } else if (line != "") {
                    owner.RegistNotes(NoteType.Bar, measureCountL);
                    MakeNotes(line, measureCountL);
                    measureCountL += owner.Measure * owner.BeatLength;
                }
            }

            owner.EndMarker = measureCountL;
        }

        /// <summary>
        /// コメントと前後の空白を除去する
        /// </summary>
        private string RemoveWaste(string line) {
            int commentIndex = line.IndexOf("//");
            if (commentIndex >= 0) {
                //コメント排除
                line = line.Substring(0, line.IndexOf("//"));
            }
            //空白も排除
            line = line.Trim();

            return line;
        }

        /// <summary>
        /// 各種設定
        /// </summary>
        private void Setting(string line) {
            //各種設定
            int colonIndex = line.IndexOf('=');
            string key = line.Substring(0, colonIndex);
            string value = line.Substring(colonIndex + 1);

            switch (key) {
                case "bpm":
                    owner.Bpm = float.Parse(value);
                    break;
                case "measure":
                    owner.Measure = float.Parse(value);
                    break;
                case "offset":
                    owner.Offset = float.Parse(value);
                    break;
                case "speed":
                    owner.Speed = float.Parse(value);
                    break;
            }
        }

        /// <summary>
        /// ノーツ配置
        /// </summary>
        private void MakeNotes(string line,float measureCountL) {
            float measureCountS = 0;
            float measurePerNote = (owner.Measure / line.Length) * owner.BeatLength;

            foreach (var x in line) {
                switch (x) {
                    case '1':
                        owner.RegistNotes(NoteType.Like, measureCountL + measureCountS );
                        break;
                    case '2':
                        owner.RegistNotes(NoteType.RT, measureCountL + measureCountS);
                        break;
                    case '3':
                        owner.RegistNotes(NoteType.Like, measureCountL + measureCountS);
                        owner.RegistNotes(NoteType.RT, measureCountL + measureCountS);
                        break;
                }
                measureCountS += measurePerNote;
            }
        }
    }
}
