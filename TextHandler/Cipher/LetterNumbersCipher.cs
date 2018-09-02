using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class LetterNumbersCipher : AbstractCipher {
        private string DecryptSingle(string encrypted) {
            if (encrypted.Length % 2 == 1) {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < encrypted.Length; i += 2) {
                if (!(char.IsDigit(encrypted[i]) && char.IsDigit(encrypted[i + 1]))) {
                    throw new ArgumentException();
                }
                sb.Append((char) ('a' + (int.Parse(encrypted[i] + "" + encrypted[i + 1]) - 1)));
            }
            return sb.ToString();
        }
        private string Decrypt(string encrypted) {
            var splitted = encrypted.Split(' ');
            for (var i = 0; i < splitted.Length; i++) {
                splitted[i] = DecryptSingle(splitted[i]);
            }
            return string.Join(" ", splitted);
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o)).ToArray();
            } catch (ArgumentException) {
                MessageBox.Show("Please, check yourself, you must use only numbers, each word's length must be even and each two digits must represent a number in range between 01 and 26");
                return new string[0];
            }
        }

        private string Encrypt(string original) {
            var lower = original.ToLower();
            var badCount = 0;
            var sb = new StringBuilder();
            for(var i = 0; i < lower.Length; i++) {
                if (Char.IsLetter(lower[i])) {
                    if (badCount > 0) {
                        sb.Append(' ');
                        badCount = 0;
                    }
                    sb.Append(Convert.ToString(lower[i] - 'a' + 1).PadLeft(2, '0'));
                } else badCount++;
            }
            return sb.ToString();
        }

        public override string[] Encrypt(string[] originalText, string addInfo) {
            return originalText.Select(o => Encrypt(o)).ToArray();
        }
    }
}
