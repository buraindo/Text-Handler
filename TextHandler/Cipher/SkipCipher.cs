using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class SkipCipher : AbstractCipher {
        private string Decrypt(string encrypted, int skipValue) {
            var output = encrypted.ToCharArray();
            for (var i = 0; i < encrypted.Length; i++) {
                var shift = (i * skipValue) % encrypted.Length;
                output[shift] = encrypted[i];
            }
            return new string(output);
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            if (string.IsNullOrEmpty(addInfo)) {
                MessageBox.Show("Please, enter a correct skip value.");
                return new string[0];
            } else {
                return encryptedText.Select(o => Decrypt(o, Convert.ToInt32(addInfo))).ToArray();
            }
        }

        private string Encrypt(string original, int skipValue) {
            var sb = new StringBuilder();
            var index = 0;
            for (var i = 0; i < original.Length; i++, index = (index + skipValue) % original.Length) {
                sb.Append(original[index]);
            }
            return sb.ToString();
        }
        public override string[] Encrypt(string[] originalText, string addInfo) {
            if (string.IsNullOrEmpty(addInfo)) {
                MessageBox.Show("Please, enter a correct skip value.");
                return new string[0];
            } else {
                return originalText.Select(o => Encrypt(o, Convert.ToInt32(addInfo))).ToArray();
            }
        }
    }
}
