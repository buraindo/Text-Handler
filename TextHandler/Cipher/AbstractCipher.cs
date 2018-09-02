namespace TextHandler.Cipher {
    abstract class AbstractCipher {
        public abstract string[] Encrypt(string[] originalText, string addInfo);
        public abstract string[] Decrypt(string[] encryptedText, string addInfo);
    }
}
