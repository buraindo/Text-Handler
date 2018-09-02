using System;
using System.Linq;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class CaesarCipher : AbstractCipher {
        private string Decrypt(string encrypted, string addInfo) {
            if (int.TryParse(addInfo, out var shiftValue) && shiftValue >= 0) {
                var output = encrypted.ToCharArray();
                for (var i = 0; i < output.Length; i++) {
                    var ch = output[i];
                    if (char.IsLetter(ch)) {
                        ch = char.ToLower(ch);
                        if (ch.IsEnglish()) {
                            var index = ch - 'a' - (shiftValue % 26) > 0 ? (ch - 'a' - (shiftValue % 26)) % 26 : (26 + (ch - 'a' - (shiftValue % 26))) % 26;
                            ch = englishAlphabet[index];
                        } else if (ch.IsRussian()) {
                            var index = ch - 'а' - (shiftValue % 33) > 0 ? (ch - 'а' - (shiftValue % 33)) % 33 : (33 + (ch - 'а' - (shiftValue % 33))) % 33;
                            ch = russianAlphabet[index];
                        }
                        if (char.IsUpper(output[i])) {
                            ch = char.ToUpper(ch);
                        }
                    }
                    output[i] = ch;
                }
                return new string(output);
            } else throw new FormatException();
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o, addInfo)).ToArray();
            } catch (FormatException) {
                MessageBox.Show("Please, enter a correct shift value");
                return new string[0];
            }
        }
        private static readonly char[] englishAlphabet = new char[26];
        private static readonly char[] russianAlphabet = new char[33];
        static CaesarCipher() {
            for (char c = 'a'; c <= 'z'; c++) {
                englishAlphabet[c - 'a'] = c;
            }
            for (char c = 'а'; c <= 'я'; c++) {
                russianAlphabet[c - 'а'] = c;
            }
        }
        private string Encrypt(string original, string addInfo) {
            if (int.TryParse(addInfo, out var shiftValue) && shiftValue >= 0) {
                var output = original.ToCharArray();
                for (var i = 0; i < output.Length; i++) {
                    var ch = char.ToLower(output[i]);
                    if (ch.IsEnglish()) {
                        ch = englishAlphabet[(ch - 'a' + shiftValue) % 26];
                    } else if (ch.IsRussian()) {
                        ch = russianAlphabet[(ch - 'а' + shiftValue) % 33];
                    }
                    if (char.IsUpper(output[i])) {
                        ch = char.ToUpper(ch);
                    }
                    output[i] = ch;
                }
                return new string(output);
            } else throw new FormatException();
        }
        public override string[] Encrypt(string[] originalText, string addInfo) {
            try {
                return originalText.Select(o => Encrypt(o, addInfo)).ToArray();
            } catch (FormatException) {
                MessageBox.Show("Please, enter a correct shift value");
                return new string[0];
            }
        }
    }
}
