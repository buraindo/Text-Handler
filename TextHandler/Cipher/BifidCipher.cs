using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextHandler.Cipher {
    class BifidCipher : AbstractCipher {
        static BifidCipher() {
            for (var i = 0; i < 9; i++) {
                for (var j = 0; j < 9; j++) {
                    Symbols[tableau[i, j]] = new Tuple<int, int>(i, j);
                }
            }
        }
        private static readonly char[,] tableau = new char[,] {
            { 'a','b','c','d','e','f','g','h','i' },
            { 'k','l','m','n','o','p','q','r','s' },
            { 't','u','v','w','x','y','z','A','B' },
            { 'C','D','E','F','G','H','I','K','L' },
            { 'M','N','O','P','Q','R','S','T','U' },
            { 'V','W','X','Y','Z','J','j','!','?' },
            { '&','\'','/','\\','|','@','"','#',';' },
            {'(',')','-','+','=','*',':',' ','%' },
            { ',','<','>','.','}','{','[',']', '_' }
        };
        private static readonly Dictionary<char, Tuple<int, int>> Symbols = new Dictionary<char, Tuple<int, int>>();
        private string Decrypt(string encrypted) {
            var encodedArr = new int[encrypted.Length * 2];
            for (int i = 0, j = 0; i < encrypted.Length; i++, j += 2) {
                var tuple = Symbols[encrypted[i]];
                encodedArr[j] = tuple.Item1;
                encodedArr[j + 1] = tuple.Item2;
            }
            var row = new int[encrypted.Length];
            var col = new int[encrypted.Length];
            Array.Copy(encodedArr, row, encrypted.Length);
            Array.Copy(encodedArr, encrypted.Length, col, 0, encrypted.Length);
            var sb = new StringBuilder();
            for (var i = 0; i < encrypted.Length; i++) {
                sb.Append(tableau[row[i], col[i]]);
            }
            return sb.ToString();
        }
        public override string[] Decrypt(string[] encryptedText, string addInfo) {
            try {
                return encryptedText.Select(o => Decrypt(o)).ToArray();
            } catch (KeyNotFoundException) {
                MessageBox.Show("Please, use only english and punctuation.");
                return new string[0];
            }
        }
        private string Encrypt(string original) {
            var arr = original.ToCharArray();
            var row = new int[original.Length];
            var col = new int[original.Length];
            for (var i = 0; i < arr.Length; i++) {
                var ch = arr[i];
                if (Symbols.ContainsKey(ch)) {
                    var tuple = Symbols[ch];
                    row[i] = tuple.Item1;
                    col[i] = tuple.Item2;
                }
            }
            var encodedArr = row.Concat(col).ToArray();
            var sb = new StringBuilder();
            for (var i = 0; i < encodedArr.Length; i += 2) {
                sb.Append(tableau[encodedArr[i], encodedArr[i + 1]]);
            }
            return sb.ToString();
        }
        public override string[] Encrypt(string[] originalText, string addInfo) {
            try {
                return originalText.Select(o => Encrypt(o)).ToArray();
            } catch (KeyNotFoundException) {
                MessageBox.Show("Please, use only english and punctuation.");
                return new string[0];
            }
        }
    }
}
