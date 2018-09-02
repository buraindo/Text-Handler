using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class MorseCipher : AbstractCipher {
        private static readonly Dictionary<char, string> MorseTo = new Dictionary<char, string>();
        private static readonly Dictionary<string, char> MorseFrom = new Dictionary<string, char>();
        private static readonly char[] keys;
        private static readonly string[] values;
        static MorseCipher() {
            keys = new char[] {
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p',
                'q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5',
                '6','7','8','9','.',',','?','-','=',':',';','(',')','/','"','$',
                '\'','\n','_','@','!','+','~','#',' ',
            };
            values = new string[] {
                ".-","-...","-.-.","-..",".","..-.","--.","....","..",".---",
                "-.-",".-..","--","-.","---",".--.","--.-",".-.","...","-","..-",
                "...-",".--","-..-","-.--","--..","-----",".----","..---",
                "...--","....-",".....","-....","--...","---..","----.",".-.-.-",
                "--..--","..--..","-....-","-...-","---...","-.-.-.","-.--.",
                "-.--.-","-..-.",".-..-.","...-..-",".----.",".-.-..","..--.-",".--.-.",
                "---.",".-.-.",".-...","...-.-","/",
            };
            for (var i = 0; i < keys.Length; i++) {
                MorseTo[keys[i]] = values[i];
                MorseFrom[values[i]] = keys[i];
            }
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o)).ToArray();
            } catch (ArgumentException) {
                MessageBox.Show("Please, check yourself, you must use only Morse code.");
                return new string[0];
            }
        }

        private string Decrypt(string encrypted) {
            try {
                var sb = new StringBuilder();
                var split = encrypted.Split(' ');
                foreach (var word in split) {
                    if (!string.IsNullOrEmpty(word)) {
                        sb.Append(MorseFrom[word]);
                    }
                }
                return sb.ToString();
            } catch (KeyNotFoundException) {
                throw new ArgumentException();
            }
        }

        public override string[] Encrypt(string[] originalText, string addInfo) {
            try {
                return originalText.Select(o => Encrypt(o.ToLower())).ToArray();
            } catch (ArgumentException) {
                MessageBox.Show("You are using some special characters which I don't know, consider using english and punctuation if possible.");
                return new string[0];
            }
        }

        private string Encrypt(string original) {
            try {
                var sb = new StringBuilder();
                foreach (var symbol in original) {
                    sb.Append(MorseTo[symbol]).Append(' ');
                }
                return sb.ToString();
            } catch (KeyNotFoundException) {
                throw new ArgumentException();
            }
        }
    }
}
