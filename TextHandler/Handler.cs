using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TextHandler.Cipher;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TextHandler {
    static class Handler {
        public enum Importer {
            Reverse,
            PigLatin,
            Palindrome,
            Statsman,
            Cipher,
            Decipher,
            /*Regex,*/
        }

        #region - Variables -
        public static string[] PigLatin_OriginalBuffer = new string[0];
        public static string[] PigLatin_TransformedBuffer = new string[0];
        public static string[] Reverse_OriginalBuffer = new string[0];
        public static string[] Reverse_ReversedBuffer = new string[0];
        public static string[] Palindrome_OriginalBuffer = new string[0];
        public static string[] Statsman_OriginalBuffer = new string[0];
        public static string[] Cipher_OriginalBuffer = new string[0];
        public static string[] Cipher_CipheredBuffer = new string[0];
        public static string[] Decipher_OriginalBuffer = new string[0];
        public static string[] Decipher_DecipheredBuffer = new string[0];
        public static string[] Regex_OriginalBuffer = new string[0];
        public static Random Random = new Random();
        #endregion

        #region - Reverse -
        public static string ReverseString(string original) {
            var chars = original.ToCharArray();
            var l = chars.Length;
            for (var i = 0; i < l / 2; i++) {
                char t = chars[i];
                chars[i] = chars[l - i - 1];
                chars[l - i - 1] = t;
            }
            return new string(chars);
        }
        public static string[] ReverseString(string[] originals) {
            var length = originals.Length;
            var output = new string[length];
            for (var i = 0; i < length; i++) {
                var original = originals[i];
                output[length - i - 1] = ReverseString(original);
            }
            return output;
        }
        #endregion

        #region - Pig Latin -
        private static readonly HashSet<char> Vowels = new HashSet<char> { 'a', 'e', 'i', 'u', 'y', 'o', };
        private static readonly IReadOnlyList<string> PigLatinDialects = new List<string> { "ay", "yay", "way" };
        public static string ToPigLatin(string original) {
            bool Incorrect() {
                if (original.Equals(string.Empty)) return true;
                var lwr = original.ToLower();
                for (var i = 0; i < lwr.Length; i++) {
                    if (!char.IsLetter(lwr[i])) {
                        return true;
                    }
                }
                return false;
            }
            string GetRandomDialect() {
                return PigLatinDialects[Random.Next(0, 3)];
            }
            if (Incorrect()) return original;
            var lower = original.ToLower();
            var first = lower[0];
            StringBuilder output;
            if (Vowels.Contains(first)) {
                output = new StringBuilder(original).Append("-" + GetRandomDialect());
            } else {
                var indexOfFirstVowel = original.IndexOf(original.First(ch => !Vowels.Contains(ch)));
                if (indexOfFirstVowel > original.Length || indexOfFirstVowel < 0) {
                    indexOfFirstVowel = original.Length - 1;
                }
                output = new StringBuilder(original.Substring(indexOfFirstVowel + 1)).Append("-" + original.Substring(0, indexOfFirstVowel + 1) + "ay");
            }
            return output.ToString();
        }
        public static string[] ToPigLatin(string[] originals) {
            var length = originals.Length;
            var output = new string[length];
            for (var j = 0; j < length; j++) {
                var original = originals[j];
                var splitted = original.Split(' ');
                var l = splitted.Length;
                var localOutput = new string[l];
                for (var i = 0; i < l; i++) {
                    var localOriginal = splitted[i];
                    localOutput[i] = ToPigLatin(localOriginal);
                }
                output[j] = string.Join(" ", localOutput);
            }
            return output;
        }
        #endregion

        #region - Palindrome -
        public static bool IsPalindrome(string[] lines) {
            var text = string.Join(string.Empty, lines);
            return text.Equals(new string(text.Reverse().ToArray()));
        }
        #endregion

        #region - Statsman -
        /// <summary>
        /// Counts general amount of words, letters and other things;
        /// Shows which words and letters were used more than others;
        /// </summary>
        /// <param name="text"></param>
        /// <returns>
        /// Returns deep statistics of the text given
        /// </returns>
        public static string[] GetStats(string[] text) {
            var letters = new Dictionary<char, int>();
            var words = new Dictionary<string, int>();
            var generalWords = 0;
            var generalLetters = 0;
            for (var i = 0; i < text.Length; i++) {
                var line = text[i].Split(' ', '(', ':', ';', ',', '.');
                for (var j = 0; j < line.Length; j++) {
                    var word = line[j].ToList();
                    for (var k = 0; k < word.Count; k++) {
                        if (!char.IsLetterOrDigit(word[k])) {
                            word.RemoveAt(k);
                        } else if (char.IsLetter(word[k])) {
                            generalLetters++;
                            if (letters.ContainsKey(word[k])) {
                                letters[word[k]]++;
                            } else letters.Add(word[k], 1);
                        }
                    }
                    if (word.Count > 0) {
                        generalWords++;
                        var wordStr = new string(word.ToArray());
                        if (words.ContainsKey(wordStr)) {
                            words[wordStr]++;
                        } else words.Add(wordStr, 1);
                    }
                }
            }
            var listLetters = letters.ToList();
            var listWords = words.ToList();
            listLetters.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            listWords.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            string[] output = new string[] {
                "This text contains: ",
                $"{generalWords} words in overall",
                $"{generalLetters} letters in overall",
                "With top-5 most used words being: ",
                $"{string.Join(Environment.NewLine, listWords.Take(5).Select(p => $"    {p.Key} : {p.Value} times met"))}",
                "With top-5 most used letters being: ",
                $"{string.Join(Environment.NewLine, listLetters.Take(5).Select(p => $"    {p.Key} : {p.Value} times met"))}",
            };
            return output;
        }
        #endregion

        #region - Cipher & Decipher -
        public static string AddInfoText;
        public static AbstractCipher GetCipher(string cipher) {
            switch (cipher) {
                case "Atbash":
                    return new AtbashCipher();
                case "Base64":
                    return new Base64Cipher();
                case "Bifid":
                    return new BifidCipher();
                case "Caesar":
                    return new CaesarCipher();
                case "Letter Numbers":
                    return new LetterNumbersCipher();
                case "Morse":
                    return new MorseCipher();
                case "Skip":
                    return new SkipCipher();
                case "Vigenere":
                    return new VigenereCipher();
                default:
                    return null;
            }
        }

        #endregion

        #region - Regex -
        public static bool/*async Task<bool>*/ Matches(string text, string pattern) {
            try {
                return Regex.IsMatch(text, pattern);
            } catch (Exception) {
                throw;
            }
        }

        #endregion
    }
}
