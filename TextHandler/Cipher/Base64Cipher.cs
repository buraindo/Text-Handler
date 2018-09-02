using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class Base64Cipher : AbstractCipher {
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o)).ToArray();
            } catch (FormatException) {
                MessageBox.Show("This string doesn't seem to be encrypted in base64, smh.");
                return new string[0];
            }
        }
        private string Decrypt(string encrypted) {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
        }
        private string Encrypt(string original) {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
        }
        public override string[] Encrypt(string[] originalText, string addInfo) {
            return originalText.Select(o => Encrypt(o)).ToArray();
        }
    }
}
