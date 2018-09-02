using System;
using System.Collections.Generic;
using static TextHandler.Handler;

namespace TextHandler {
    static class Util {

        #region - Variables -
        private static Dictionary<Importer, Action<string[]>> SetOriginalBufferByImporter;
        private static Dictionary<Importer, Action> SetTextBoxLinesByImporter;
        #endregion

        #region - Extensions -

        public static bool IsRussian(this char ch) {
            return (ch >= 'а' && ch <= 'я') | (ch >= 'А' && ch <= 'Я');
        }
        public static bool IsEnglish(this char ch) {
            return (ch >= 'a' && ch <= 'z') | (ch >= 'A' && ch <= 'Z');
        }

        #endregion
        private static void Awake() {
            SetOriginalBufferByImporter = new Dictionary<Importer, Action<string[]>>();
            SetTextBoxLinesByImporter = new Dictionary<Importer, Action>();
        }
        private static void Fill() {
            SetOriginalBufferByImporter.Add(Importer.Reverse, (newValue) => Reverse_OriginalBuffer = newValue);
            SetOriginalBufferByImporter.Add(Importer.PigLatin, (newValue) => PigLatin_OriginalBuffer = newValue);
            SetOriginalBufferByImporter.Add(Importer.Palindrome, (newValue) => Palindrome_OriginalBuffer = newValue);
            SetOriginalBufferByImporter.Add(Importer.Statsman, (newValue) => Statsman_OriginalBuffer = newValue);
            SetOriginalBufferByImporter.Add(Importer.Cipher, (newValue) => Cipher_OriginalBuffer = newValue);
            SetOriginalBufferByImporter.Add(Importer.Decipher, (newValue) => Decipher_OriginalBuffer = newValue);
            //SetOriginalBufferByImporter.Add(Importer.Regex, (newValue) => Regex_OriginalBuffer = newValue);
        }
        public static string[] GetEdited(Importer importer) {
            switch (importer) {
                case Importer.Reverse:
                    return Reverse_ReversedBuffer;
                case Importer.PigLatin:
                    return PigLatin_TransformedBuffer;
                case Importer.Cipher:
                    return Cipher_CipheredBuffer;
                case Importer.Decipher:
                    return Decipher_DecipheredBuffer;
                default:
                    return new string[0];
            }
        }
        public static void SetOriginal(Importer importer, string[] value) {
            SetOriginalBufferByImporter[importer](value);
        }
        public static void SetInputTextBoxLines(Importer importer) {
            SetTextBoxLinesByImporter[importer]();
        }
        public static void Fill(Importer[] keys, Action[] values) {
            for(var i = 0; i < keys.Length; i++) {
                var key = keys[i];
                var value = values[i];
                SetTextBoxLinesByImporter.Add(key, value);
            }
        }
        public static void Init() {
            Awake();
            Fill();
        }
    }
}
