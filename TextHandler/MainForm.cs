using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TextHandler.Handler;
using System.Collections;
using System.Text.RegularExpressions;

namespace TextHandler {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            Util.Init();
            Init();
        }

        #region - Delegates for textBoxes -
        private void setReverseInputTextBoxLinesMethod() => Reverse_InputTextBox.Lines = Reverse_OriginalBuffer.Take(12).ToArray();
        private void setPigLatinInputTextBoxLinesMethod() => PigLatin_InputTextBox.Lines = PigLatin_OriginalBuffer.Take(12).ToArray();
        private void setPalindromeInputTextBoxLinesMethod() => Palindrome_InputTextBox.Lines = Palindrome_OriginalBuffer.Take(12).ToArray();
        private void setStatsmanInputTextBoxLinesMethod() => Statsman_InputTextBox.Lines = Statsman_OriginalBuffer.Take(12).ToArray();
        private void setCipherInputTextBoxLinesMethod() => Cipher_InputTextBox.Lines = Cipher_OriginalBuffer.Take(12).ToArray();
        private void setDecipherInputTextBoxLinesMethod() => Decipher_InputTextBox.Lines = Decipher_OriginalBuffer.Take(12).ToArray();
        //private void setRegexInputTextBoxLinesMethod() => Regex_InputTextBox.Lines = Regex_OriginalBuffer.Take(12).ToArray();
        private Action setReverseInputTextBoxLines;
        private Action setPigLatinInputTextBoxLines;
        private Action setPalindromeInputTextBoxLines;
        private Action setStatsmanInputTextBoxLines;
        private Action setCipherInputTextBoxLines;
        private Action setDecipherInputTextBoxLines;
        private Action setRegexInputTextBoxLines;
        #endregion

        #region - Initialization -
        private void InitDelegates() {
            setReverseInputTextBoxLines = setReverseInputTextBoxLinesMethod;
            setPigLatinInputTextBoxLines = setPigLatinInputTextBoxLinesMethod;
            setPalindromeInputTextBoxLines = setPalindromeInputTextBoxLinesMethod;
            setStatsmanInputTextBoxLines = setStatsmanInputTextBoxLinesMethod;
            setCipherInputTextBoxLines = setCipherInputTextBoxLinesMethod;
            setDecipherInputTextBoxLines = setDecipherInputTextBoxLinesMethod;
            //setRegexInputTextBoxLines = setRegexInputTextBoxLinesMethod;
        }
        private void Init() {
            InitDelegates();
            Util.Fill((Importer[]) Enum.GetValues(typeof(Importer)), new Action[] { setReverseInputTextBoxLines, setPigLatinInputTextBoxLines, setPalindromeInputTextBoxLines, setStatsmanInputTextBoxLines, setCipherInputTextBoxLines, setDecipherInputTextBoxLines, /*setRegexInputTextBoxLines*/});
        }
        #endregion

        #region - Helpful methods -
        private async Task HandleImport(Importer importer) {
            var dialog = new OpenFileDialog {
                Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*",
                Multiselect = false,
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                var importTask = new Task<string[]>(() => {
                    var path = dialog.FileName;
                    Encoding encoding;
                    try {
                        encoding = Encoding.GetEncoding(GetComboBox(importer).SelectedItem.ToString().ToLower());
                    } catch (Exception ex)
                       when (ex is ArgumentException ||
                             ex is NullReferenceException) {
                        encoding = Encoding.UTF8;
                    }
                    return File.ReadAllLines(path, encoding);
                });
                importTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
                Util.SetOriginal(importer, await importTask);
                Util.SetInputTextBoxLines(importer);
            }
        }
        private void HandleSave(Importer importer) {
            var dialog = new SaveFileDialog {
                Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*",
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                var path = dialog.FileName;
                var saveTask = Task.Run(() => {
                    File.WriteAllLines(path, Util.GetEdited(importer));
                });
            }
        }
        private ComboBox GetComboBox(Importer i) {
            switch (i) {
                case Importer.PigLatin:
                    return PigLatin_EncodingComboBox;
                case Importer.Reverse:
                    return Reverse_EncodingComboBox;
                case Importer.Palindrome:
                    return Palindrome_EncodingComboBox;
                case Importer.Statsman:
                    return Statsman_EncodingComboBox;
                case Importer.Cipher:
                    return Cipher_EncodingComboBox;
                case Importer.Decipher:
                    return Decipher_EncodingComboBox;
                default:
                    return null;
            }
        }
        #endregion

        #region - Reverse -
        private async void Reverse_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.Reverse);
        }
        private async void Reverse_ReverseButton_Click(object sender, EventArgs e) {
            await Task.Run(() => Reverse_ReversedBuffer = ReverseString(Reverse_OriginalBuffer.Length > 0 ? Reverse_OriginalBuffer : Reverse_InputTextBox.Lines));
            Reverse_OutputTextBox.Lines = Reverse_ReversedBuffer.Take(15).ToArray();
        }

        private void Reverse_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { Reverse_InputTextBox.Clear(); Reverse_OutputTextBox.Clear(); Reverse_OriginalBuffer = new string[0]; Reverse_ReversedBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Reverse_SaveButton_Click(object sender, EventArgs e) {
            HandleSave(Importer.Reverse);
        }

        private void Reverse_CopyButton_Click(object sender, EventArgs e) {
            if (Reverse_ReversedBuffer.Length > 0) {
                Clipboard.SetText(string.Join(Environment.NewLine, Reverse_ReversedBuffer));
            }
        }

        private void Reverse_InputTextBox_TextChanged(object sender, EventArgs e) {
            Reverse_OriginalBuffer = new string[0];
        }

        #endregion

        #region - Pig Latin -
        private async void PigLatin_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.PigLatin);
        }

        private async void PigLatin_GoButton_Click(object sender, EventArgs e) {
            await Task.Run(() => PigLatin_TransformedBuffer = ToPigLatin(PigLatin_OriginalBuffer.Length > 0 ? PigLatin_OriginalBuffer : PigLatin_InputTextBox.Lines));
            PigLatin_OutputTextBox.Lines = PigLatin_TransformedBuffer.Take(15).ToArray();
        }

        private void PigLatin_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { PigLatin_InputTextBox.Clear(); PigLatin_OutputTextBox.Clear(); PigLatin_OriginalBuffer = new string[0]; PigLatin_TransformedBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void PigLatin_SaveButton_Click(object sender, EventArgs e) {
            HandleSave(Importer.PigLatin);
        }

        private void PigLatin_CopyButton_Click(object sender, EventArgs e) {
            if (PigLatin_TransformedBuffer.Length > 0) {
                Clipboard.SetText(string.Join(Environment.NewLine, PigLatin_TransformedBuffer));
            }
        }

        private void PigLatin_InputTextBox_TextChanged(object sender, EventArgs e) {
            PigLatin_OriginalBuffer = new string[0];
        }

        #endregion

        #region - Palindrome -
        private async void Palindrome_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.Palindrome);
        }

        private async void Palindrome_CheckButton_Click(object sender, EventArgs e) {
            var answer = await Task.Run(() => IsPalindrome(Palindrome_OriginalBuffer.Length > 0 ? Palindrome_OriginalBuffer : Palindrome_InputTextBox.Lines));
            Palindrome_OutputTextBox.Lines = new string[] { answer.ToString() };
        }

        private void Palindrome_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { Palindrome_InputTextBox.Clear(); Palindrome_OutputTextBox.Clear(); Palindrome_OriginalBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Palindrome_InputTextBox_TextChanged(object sender, EventArgs e) {
            Palindrome_OriginalBuffer = new string[0];
        }

        #endregion

        #region - Statsman -
        private async void Statsman_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.Statsman);
        }

        private async void Statsman_GetStatsButton_Click(object sender, EventArgs e) {
            var stats = await Task.Run(() => GetStats(Statsman_OriginalBuffer.Length > 0 ? Statsman_OriginalBuffer : Statsman_InputTextBox.Lines));
            Statsman_OutputTextBox.Lines = stats;
        }

        private void Statsman_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { Statsman_InputTextBox.Clear(); Statsman_OutputTextBox.Clear(); Statsman_OriginalBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Statsman_CopyButton_Click(object sender, EventArgs e) {
            if (Statsman_OutputTextBox.Lines.Length > 0) {
                Clipboard.SetText(string.Join(Environment.NewLine, Statsman_OutputTextBox.Lines));
            }
        }

        private void Statsman_InputTextBox_TextChanged(object sender, EventArgs e) {
            Statsman_OriginalBuffer = new string[0];
        }

        #endregion

        #region - Cipher -
        private async void Cipher_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.Cipher);
        }

        private async void Cipher_EncryptButton_Click(object sender, EventArgs e) {
            var cipherType = Cipher_CipherComboBox.SelectedItem?.ToString();
            var cipher = cipherType != null ? GetCipher(cipherType) : GetCipher("Atbash");
            var addInfo = Cipher_SkipValuesComboBox.Visible ? Cipher_SkipValuesComboBox.SelectedItem?.ToString() : Cipher_AddInfoTextBox.Text;
            await Task.Run(() => Cipher_CipheredBuffer = cipher.Encrypt(Cipher_OriginalBuffer.Length > 0 ? Cipher_OriginalBuffer : Cipher_InputTextBox.Lines, addInfo));
            Cipher_OutputTextBox.Lines = Cipher_CipheredBuffer.Take(15).ToArray();
        }

        private void Cipher_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { Cipher_InputTextBox.Clear(); Cipher_OutputTextBox.Clear(); Cipher_OriginalBuffer = new string[0]; Cipher_CipheredBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Cipher_SaveButton_Click(object sender, EventArgs e) {
            HandleSave(Importer.Cipher);
        }

        private void Cipher_CopyButton_Click(object sender, EventArgs e) {
            if (Cipher_OutputTextBox.Lines.Length > 0) {
                Clipboard.SetText(string.Join(Environment.NewLine, Cipher_OutputTextBox.Lines));
            }
        }

        private void Cipher_CipherCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            void ChangeAddInfoText(string text) {
                AddInfoText = text;
                Cipher_AddInfoTextBox.Visible = true;
                Cipher_AddInfoTextBox.Text = AddInfoText;
                Cipher_SkipValuesComboBox.Visible = false;
            }
            switch (Cipher_CipherComboBox.SelectedItem.ToString()) {
                case "Caesar":
                    ChangeAddInfoText("Shift value...");
                    break;
                case "Vigenere":
                    ChangeAddInfoText("Passphrase...");
                    break;
                case "Skip":
                    Cipher_AddInfoTextBox.Visible = false;
                    Cipher_SkipValuesComboBox.Visible = true;
                    Cipher_SetSkipValues();
                    break;
                default:
                    Cipher_AddInfoTextBox.Visible = false;
                    Decipher_SkipValuesComboBox.Visible = false;
                    break;
            }
        }
        private void Cipher_SetSkipValues() {
            var totalLength = Cipher_InputTextBox.Lines.Select(l => l.Length).Sum();
            var range = Enumerable.Range(1, totalLength).Where(n => (totalLength % n != 0 || n == 1)).ToList();
            for (var i = 0; i < Cipher_SkipValuesComboBox.Items.Count; i++) {
                if ((int) Cipher_SkipValuesComboBox.Items[i] > totalLength || totalLength % (int) Cipher_SkipValuesComboBox.Items[i] == 0) {
                    Cipher_SkipValuesComboBox.Items.RemoveAt(i);
                }
            }
            range.ForEach(n => {
                if (!Cipher_SkipValuesComboBox.Items.Contains(n)) {
                    Cipher_SkipValuesComboBox.Items.Add(n);
                }
            });
        }
        private void Cipher_InputTextBox_TextChanged(object sender, EventArgs e) {
            Cipher_OriginalBuffer = new string[0];
            if (Cipher_SkipValuesComboBox.Visible) {
                Cipher_SetSkipValues();
            }
        }

        private void Cipher_AddInfoTextBox_Leave(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Cipher_AddInfoTextBox.Text)) {
                Cipher_AddInfoTextBox.Text = AddInfoText;
            }
        }
        private void Cipher_AddInfoTextBox_Enter(object sender, EventArgs e) {
            if (Cipher_AddInfoTextBox.Text.Equals(AddInfoText)) {
                Cipher_AddInfoTextBox.Text = string.Empty;
            }
        }

        #endregion

        #region - Decipher -

        private void Decipher_AddInfoTextBox_Leave(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Decipher_AddInfoTextBox.Text)) {
                Decipher_AddInfoTextBox.Text = AddInfoText;
            }
        }
        private void Decipher_AddInfoTextBox_Enter(object sender, EventArgs e) {
            if (Decipher_AddInfoTextBox.Text.Equals(AddInfoText)) {
                Decipher_AddInfoTextBox.Text = string.Empty;
            }
        }
        private void Decipher_CipherComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            void ChangeAddInfoText(string text) {
                AddInfoText = text;
                Decipher_AddInfoTextBox.Visible = true;
                Decipher_AddInfoTextBox.Text = AddInfoText;
                Decipher_SkipValuesComboBox.Visible = false;
            }
            switch (Decipher_CipherComboBox.SelectedItem.ToString()) {
                case "Caesar":
                    ChangeAddInfoText("Shift value...");
                    break;
                case "Vigenere":
                    ChangeAddInfoText("Passphrase...");
                    break;
                case "Skip":
                    Decipher_SkipValuesComboBox.Visible = true;
                    Decipher_SetSkipValues();
                    Decipher_AddInfoTextBox.Visible = false;
                    break;
                default:
                    Decipher_AddInfoTextBox.Visible = false;
                    Decipher_SkipValuesComboBox.Visible = false;
                    break;
            }
        }

        private async void Decipher_ImportButton_Click(object sender, EventArgs e) {
            await HandleImport(Importer.Decipher);
        }

        private async void Decipher_DecryptButton_Click(object sender, EventArgs e) {
            var cipherType = Decipher_CipherComboBox.SelectedItem?.ToString();
            var decipher = cipherType != null ? GetCipher(cipherType) : GetCipher("Atbash");
            var addInfo = Decipher_SkipValuesComboBox.Visible ? Decipher_SkipValuesComboBox.SelectedItem?.ToString() : Decipher_AddInfoTextBox.Text;
            await Task.Run(() => Decipher_DecipheredBuffer = decipher.Decrypt(Decipher_OriginalBuffer.Length > 0 ? Decipher_OriginalBuffer : Decipher_InputTextBox.Lines, addInfo));
            Decipher_OutputTextBox.Lines = Decipher_DecipheredBuffer.Take(15).ToArray();
        }

        private void Decipher_ClearButton_Click(object sender, EventArgs e) {
            var clearTask = new Task(() => { Decipher_InputTextBox.Clear(); Decipher_OutputTextBox.Clear(); Decipher_OriginalBuffer = new string[0]; Decipher_DecipheredBuffer = new string[0]; });
            clearTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Decipher_SaveButton_Click(object sender, EventArgs e) {
            HandleSave(Importer.Decipher);
        }

        private void Decipher_CopyButton_Click(object sender, EventArgs e) {
            if (Decipher_OutputTextBox.Lines.Length > 0) {
                Clipboard.SetText(string.Join(Environment.NewLine, Decipher_OutputTextBox.Lines));
            }
        }
        private void Decipher_SetSkipValues() {
            var totalLength = Decipher_InputTextBox.Lines.Select(l => l.Length).Sum();
            var range = Enumerable.Range(1, totalLength).Where(n => (totalLength % n != 0 || n == 1)).ToList();
            for (var i = 0; i < Decipher_SkipValuesComboBox.Items.Count; i++) {
                if ((int) Decipher_SkipValuesComboBox.Items[i] > totalLength || totalLength % (int) Decipher_SkipValuesComboBox.Items[i] == 0) {
                    Decipher_SkipValuesComboBox.Items.RemoveAt(i);
                }
            }
            range.ForEach(n => {
                if (!Decipher_SkipValuesComboBox.Items.Contains(n)) {
                    Decipher_SkipValuesComboBox.Items.Add(n);
                }
            });
        }
        private void Decipher_InputTextBox_TextChanged(object sender, EventArgs e) {
            Decipher_OriginalBuffer = new string[0];
            if (Decipher_SkipValuesComboBox.Visible) {
                Decipher_SetSkipValues();
            }
        }

        #endregion

        #region - Regex -

        private void Regex_InputTextBox_TextChanged(object sender, EventArgs e) {
            try {
                Regex_MatchesTextBox.Text = Convert.ToString(Matches(Regex_InputTextBox.Text, Regex_RegexTextBox.Text));
            } catch (ArgumentException) {
                MessageBox.Show("You've typed an inappropriate pattern, make sure to type a correct one.");
            }
        }

        private void Regex_RegexTextBox_TextChanged(object sender, EventArgs e) {
            try {
                Regex_MatchesTextBox.Text = Convert.ToString(Matches(Regex_InputTextBox.Text, Regex_RegexTextBox.Text));
            } catch (ArgumentException) {
                MessageBox.Show("You've typed an inappropriate pattern, make sure to type a correct one.");
            }
        }
        private void Regex_InputTextBox_Leave(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Regex_InputTextBox.Text)) {
                Regex_InputTextBox.Text = "Input text here...";
            }
        }
        private void Regex_InputTextBox_Enter(object sender, EventArgs e) {
            if (Regex_InputTextBox.Text.Equals("Input text here...")) {
                Regex_InputTextBox.Text = string.Empty;
            }
        }
        private void Regex_RegexTextBox_Leave(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Regex_RegexTextBox.Text)) {
                Regex_RegexTextBox.Text = "Regex...";
            }
        }
        private void Regex_RegexTextBox_Enter(object sender, EventArgs e) {
            if (Regex_RegexTextBox.Text.Equals("Regex...")) {
                Regex_RegexTextBox.Text = string.Empty;
            }
        }

        #endregion
    }
}
