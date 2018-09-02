using System;
using System.Linq;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class VigenereCipher : AbstractCipher {
        private string Decrypt(string encrypted, string addInfo) {
            if (string.IsNullOrEmpty(addInfo)) {
                throw new FormatException();
            }
            var output = encrypted.ToCharArray();
            var currentIndex = 0;
            var shifts = addInfo.ToLower().Where(ch => char.IsLetter(ch)).Select(ch => ch.IsEnglish() ? ch - 'a' : ch - 'а').ToArray();
            for (var i = 0; i < output.Length; i++) {
                var ch = output[i];
                if (char.IsLetter(ch)) {
                    ch = char.ToLower(ch);
                    if (ch.IsEnglish()) {
                        var index = ch - 'a' - (shifts[currentIndex % shifts.Length] % 26) > 0 ? (ch - 'a' - (shifts[currentIndex % shifts.Length] % 26)) % 26 : (26 + (ch - 'a' - (shifts[currentIndex % shifts.Length] % 26))) % 26;
                        currentIndex++;
                        ch = englishAlphabet[index];
                    } else if (ch.IsRussian()) {
                        var index = ch - 'а' - (shifts[currentIndex % shifts.Length] % 33) > 0 ? (ch - 'а' - (shifts[currentIndex % shifts.Length] % 33)) % 33 : (33 + (ch - 'а' - (shifts[currentIndex % shifts.Length] % 33))) % 33;
                        currentIndex++;
                        ch = russianAlphabet[index];
                    }
                    if (char.IsUpper(output[i])) {
                        ch = char.ToUpper(ch);
                    }
                }
                output[i] = ch;
            }
            return new string(output);
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o, addInfo)).ToArray();
            } catch (FormatException) {
                MessageBox.Show("Please, enter a correct passphrase");
                return new string[0];
            }
        }
        private static readonly char[] englishAlphabet = new char[26];
        private static readonly char[] russianAlphabet = new char[33];
        static VigenereCipher() {
            for (char c = 'a'; c <= 'z'; c++) {
                englishAlphabet[c - 'a'] = c;
            }
            for (char c = 'а'; c <= 'я'; c++) {
                russianAlphabet[c - 'а'] = c;
            }
        }
        private string Encrypt(string original, string addInfo) {
            if (string.IsNullOrEmpty(addInfo)) {
                throw new FormatException();
            }
            var output = original.ToCharArray();
            var index = 0;
            var shifts = addInfo.ToLower().Where(ch => char.IsLetter(ch)).Select(ch => ch.IsEnglish() ? ch - 'a' : ch - 'а').ToArray();
            for (var i = 0; i < output.Length; i++) {
                var ch = char.ToLower(output[i]);
                if (ch.IsEnglish()) {
                    ch = englishAlphabet[(ch - 'a' + shifts[index++ % shifts.Length]) % 26];
                } else if (ch.IsRussian()) {
                    ch = russianAlphabet[(ch - 'а' + shifts[index++ % shifts.Length]) % 33];
                }
                if (char.IsUpper(output[i])) {
                    ch = char.ToUpper(ch);
                }
                output[i] = ch;
            }
            return new string(output);
        }
        public override string[] Encrypt(string[] originalText, string addInfo) {
            try {
                return originalText.Select(o => Encrypt(o, addInfo)).ToArray();
            } catch (FormatException) {
                MessageBox.Show("Please, enter a correct passphrase");
                return new string[0];
            }
        }
    }
}
