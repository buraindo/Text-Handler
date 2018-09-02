using System;
using System.Linq;

namespace TextHandler.Cipher {
    class AtbashCipher : AbstractCipher {
        private string Encrypt(char[] original) {
            for (var i = 0; i < original.Length; i++) {
                if (char.IsLetter(original[i])) {
                    if (char.IsUpper(original[i])) {
                        original[i] = (char)('Z' - (original[i] - 'A'));
                    } else {
                        original[i] = (char) ('z' - (original[i] - 'a'));
                    }
                }
            }
            return new string(original);
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            return Encrypt(encryptedText, addInfo);
        }

        public override string[] Encrypt(string[] originalText, string addInfo) {
            return originalText.Select(o => Encrypt(o.ToCharArray())).ToArray();
        }
    }
}
