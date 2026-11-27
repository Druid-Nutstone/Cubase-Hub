using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Lex;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Cubase.Macro.Forms.Scripts.Editor
{
    public class ScriptEditor : RichTextBox
    {
        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private readonly IServiceProvider serviceProvider;
        private readonly IScriptService scriptService;
        private readonly HashSet<string> scriptCommands = new(StringComparer.OrdinalIgnoreCase);

        private CubaseScriptCommandCollection functions;

        private CubaseKeyCommandCollection midiFunctions;

        private static readonly HashSet<string> Keywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "if",
            "then",
            "else",
            "end",
            "end if",
            "var"
        };

        private bool isColorizing;
        private bool skipColorizeOnNextChange;

        private bool forceFullColorize;
        private char lastKeyChar;
        private static readonly Regex CommentRegex = new(
            @"(?://|\\\\).*$",
            RegexOptions.Multiline | RegexOptions.Compiled);

        private static readonly Regex StringLiteralRegex = new(
            @"""(?:\\.|[^""\\\r\n])*""",
            RegexOptions.Compiled);

        private static readonly Regex KeywordRegex = new(
            @"\bend\s+if\b|\b(?:if|then|else|var)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex FunctionCallRegex = new(
            @"\b([A-Za-z_][A-Za-z0-9_]*)\s*(?=\()",
            RegexOptions.Compiled);

        private Color DefaultTextColor { get; set; } = Color.White;
        private Color CommentColor { get; set; } = Color.Green;
        private Color StringColor { get; set; } = Color.SandyBrown;
        private Color FunctionColor { get; set; } = Color.Cyan;
        private Color KeywordColor { get; set; } = Color.Yellow;

        private Action<string> OnError;

        private MacroParser macroParser;

        private LexParser lexParser;

        public ScriptEditor(IServiceProvider serviceProvider,
                            Action<string> onError,
                            string text) : base()
        {
            this.serviceProvider = serviceProvider;
            this.OnError = onError;
            this.scriptService = this.serviceProvider?.GetService<IScriptService>();
            this.BorderStyle = BorderStyle.None;
            this.Font = new Font(this.Font.FontFamily, 12);
            LoadScriptCommands();
            this.ContextMenuStrip = new EditorContextMenu(this, this.functions, this.midiFunctions);
            if (string.IsNullOrEmpty(text))
            {
                this.InitialiseScript();
            }
            else
            {
                this.Text = text;
            }

            this.ColorizeText();
        }

        public void LoadScriptText(string scriptContent)
        {
            this.Text = scriptContent;
            this.ColorizeText();
        }

        public void NewScript()
        {
            this.Text = string.Empty;
            this.InitialiseScript();
            this.ColorizeText();
        }

        public void InsertText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            this.SelectedText = text;
            this.Focus();
            this.ColorizeText();
        }

        private void InitialiseScript()
        {
            this.Text = $"[Description(\"Description here\")]{Environment.NewLine}[Name(\"Name of the script here\")]{Environment.NewLine}";
        }

        private void LoadScriptCommands()
        {
            this.scriptCommands.Clear();

            this.functions = this.scriptService?.GetScriptCommands();
            this.functions?.ForEach((x) => { this.scriptCommands.Add(x.Name); });
            this.midiFunctions = this.scriptService?.GetMidiCommands();
            this.midiFunctions?.ForEach((x) => { this.scriptCommands.Add(x.Name); });
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            this.lastKeyChar = e.KeyChar;
            base.OnKeyPress(e);

            if (e.KeyChar == '\r' || e.KeyChar == (char)Keys.Enter)
            {
                this.forceFullColorize = true;
                this.VerifyScript();
                IndentToPreviousLineFirstWord();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Tab)
            {
                this.skipColorizeOnNextChange = true;
            }
            else if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                // Deleting or inserting a newline can shift or merge lines
                this.forceFullColorize = true;
            }
        }

        private void IndentToPreviousLineFirstWord()
        {
            this.SuspendLayout();
            int caretIndex = this.SelectionStart;
            int currentLineIndex = this.GetLineFromCharIndex(caretIndex);

            if (currentLineIndex <= 0)
            {
                return;
            }

            int previousLineIndex = currentLineIndex - 1;
            int prevLineStart = this.GetFirstCharIndexFromLine(previousLineIndex);
            int currentLineStart = this.GetFirstCharIndexFromLine(currentLineIndex);

            if (prevLineStart < 0 || currentLineStart <= prevLineStart)
            {
                return;
            }

            // Read text of the previous line (excluding trailing \r or \n)
            string prevLineText = this.Text.Substring(prevLineStart, currentLineStart - prevLineStart)
                                           .TrimEnd('\r', '\n');

            // Count leading whitespace up to the first non-whitespace character
            int leadingSpaces = 0;
            while (leadingSpaces < prevLineText.Length && char.IsWhiteSpace(prevLineText[leadingSpaces]))
            {
                leadingSpaces++;
            }

            // Do not indent if the previous line is blank/whitespace-only
            if (leadingSpaces > 0 && leadingSpaces < prevLineText.Length)
            {
                string spaces = new string(' ', leadingSpaces);
                this.SelectedText = spaces;
            }
            this.ResumeLayout();
        }

        private void VerifyScript()
        {
            var lexParser = new LexParser();
            var tokens = lexParser.Parse(this.Text);
            var macroParser = new MacroParser(tokens);
            try
            {
                macroParser.Parse();
            }
            catch (Exception ex)
            {
                this.OnError.Invoke(ex.Message);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.OnError?.Invoke(string.Empty);

            if (this.isColorizing)
            {
                return;
            }

            if (this.skipColorizeOnNextChange)
            {
                this.skipColorizeOnNextChange = false;
                this.forceFullColorize = false;
                return;
            }

            if (this.forceFullColorize)
            {
                this.forceFullColorize = false;
                ColorizeText();
                return;
            }

            if (this.lastKeyChar != '\0' && !IsSyntaxRelevantChar(this.lastKeyChar))
            {
                this.lastKeyChar = '\0';
                return;
            }

            this.lastKeyChar = '\0';
            ColorizeCurrentLine();
        }

        private void ColorizeText()
        {
            this.SuspendLayout();
            if (this.isColorizing || string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            this.isColorizing = true;
            int originalStart = this.SelectionStart;
            int originalLength = this.SelectionLength;

            try
            {
                if (this.IsHandleCreated)
                {
                    SendMessage(this.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                }

                string text = this.Text;

                // 1. Reset full document to default color
                this.SelectAll();
                this.SelectionColor = this.DefaultTextColor;

                // Track comment and string spans so keywords/functions inside them are not recolored
                var protectedSpans = new List<(int Start, int End)>();

                // 2. Color comments first (// and \\)
                foreach (Match match in CommentRegex.Matches(text))
                {
                    ApplyColor(match.Index, match.Length, this.CommentColor);
                    protectedSpans.Add((match.Index, match.Index + match.Length));
                }

                // 3. Color double-quoted string literals outside comments
                foreach (Match match in StringLiteralRegex.Matches(text))
                {
                    if (!IsInsideSpan(match.Index, protectedSpans))
                    {
                        ApplyColor(match.Index, match.Length, this.StringColor);
                        protectedSpans.Add((match.Index, match.Index + match.Length));
                    }
                }

                // 4. Color keywords (if, then, end if, else)
                foreach (Match match in KeywordRegex.Matches(text))
                {
                    if (!IsInsideSpan(match.Index, protectedSpans))
                    {
                        ApplyColor(match.Index, match.Length, this.KeywordColor);
                    }
                }

                // 5. Color function / command names (never overwriting keywords, comments, or strings)
                foreach (Match match in FunctionCallRegex.Matches(text))
                {
                    Group nameGroup = match.Groups[1];
                    if (IsInsideSpan(nameGroup.Index, protectedSpans) || Keywords.Contains(nameGroup.Value))
                    {
                        continue;
                    }

                    if (this.scriptCommands.Count == 0 || this.scriptCommands.Contains(nameGroup.Value))
                    {
                        ApplyColor(nameGroup.Index, nameGroup.Length, this.FunctionColor);
                    }
                }

                // Highlight known commands from IScriptService even if called without parentheses
                if (this.scriptCommands.Count > 0)
                {
                    string pattern = @"\b(" + string.Join("|", this.scriptCommands.Select(Regex.Escape)) + @")\b";
                    foreach (Match match in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
                    {
                        if (!IsInsideSpan(match.Index, protectedSpans) && !Keywords.Contains(match.Value))
                        {
                            ApplyColor(match.Index, match.Length, this.FunctionColor);
                        }
                    }
                }
            }
            finally
            {
                int safeStart = Math.Min(originalStart, this.TextLength);
                int safeLength = Math.Min(originalLength, Math.Max(0, this.TextLength - safeStart));

                this.SelectionStart = safeStart;
                this.SelectionLength = safeLength;

                if (this.IsHandleCreated)
                {
                    SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
                    this.Refresh();
                }

                this.isColorizing = false;
            }
            this.ResumeLayout();
        }

        private void ColorizeCurrentLine()
        {
            if (this.isColorizing || string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            int caretIndex = Math.Min(this.SelectionStart, Math.Max(0, this.TextLength - 1));
            int lineIndex = this.GetLineFromCharIndex(caretIndex);
            int lineStart = this.GetFirstCharIndexFromLine(lineIndex);

            if (lineStart < 0)
            {
                ColorizeText();
                return;
            }

            // Find end of current line (up to \r or \n)
            int nextLineStart = (lineIndex + 1 < this.Lines.Length)
                ? this.GetFirstCharIndexFromLine(lineIndex + 1)
                : this.TextLength;

            if (nextLineStart < lineStart)
            {
                nextLineStart = this.TextLength;
            }

            string rawSlice = this.Text.Substring(lineStart, nextLineStart - lineStart);
            string lineText = rawSlice.TrimEnd('\r', '\n');
            int lineLength = lineText.Length;

            if (lineLength == 0)
            {
                // Reset empty line to default color without touching other lines
                this.isColorizing = true;
                int origStart = this.SelectionStart;
                int origLen = this.SelectionLength;
                try
                {
                    if (this.IsHandleCreated)
                    {
                        SendMessage(this.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                    }

                    ApplyColor(lineStart, Math.Max(1, rawSlice.Length), this.DefaultTextColor);
                }
                finally
                {
                    this.SelectionStart = Math.Min(origStart, this.TextLength);
                    this.SelectionLength = Math.Min(origLen, Math.Max(0, this.TextLength - this.SelectionStart));
                    if (this.IsHandleCreated)
                    {
                        SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
                        this.Refresh();
                    }
                    this.isColorizing = false;
                }
                return;
            }

            this.isColorizing = true;
            int originalStart = this.SelectionStart;
            int originalLength = this.SelectionLength;

            try
            {
                if (this.IsHandleCreated)
                {
                    SendMessage(this.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                }

                // 1. Reset only the current line to default color
                ApplyColor(lineStart, lineLength, this.DefaultTextColor);

                var protectedSpans = new List<(int Start, int End)>();

                // 2. Comments on this line
                foreach (Match match in CommentRegex.Matches(lineText))
                {
                    int absStart = lineStart + match.Index;
                    ApplyColor(absStart, match.Length, this.CommentColor);
                    protectedSpans.Add((absStart, absStart + match.Length));
                }

                // 3. String literals on this line
                foreach (Match match in StringLiteralRegex.Matches(lineText))
                {
                    int absStart = lineStart + match.Index;
                    if (!IsInsideSpan(absStart, protectedSpans))
                    {
                        ApplyColor(absStart, match.Length, this.StringColor);
                        protectedSpans.Add((absStart, absStart + match.Length));
                    }
                }

                // 4. Keywords on this line
                foreach (Match match in KeywordRegex.Matches(lineText))
                {
                    int absStart = lineStart + match.Index;
                    if (!IsInsideSpan(absStart, protectedSpans))
                    {
                        ApplyColor(absStart, match.Length, this.KeywordColor);
                    }
                }

                // 5. Function calls with parentheses on this line
                foreach (Match match in FunctionCallRegex.Matches(lineText))
                {
                    Group nameGroup = match.Groups[1];
                    int absStart = lineStart + nameGroup.Index;
                    if (IsInsideSpan(absStart, protectedSpans) || Keywords.Contains(nameGroup.Value))
                    {
                        continue;
                    }

                    if (this.scriptCommands.Count == 0 || this.scriptCommands.Contains(nameGroup.Value))
                    {
                        ApplyColor(absStart, nameGroup.Length, this.FunctionColor);
                    }
                }

                // 6. Known commands from IScriptService on this line
                if (this.scriptCommands.Count > 0)
                {
                    string pattern = @"\b(" + string.Join("|", this.scriptCommands.Select(Regex.Escape)) + @")\b";
                    foreach (Match match in Regex.Matches(lineText, pattern, RegexOptions.IgnoreCase))
                    {
                        int absStart = lineStart + match.Index;
                        if (!IsInsideSpan(absStart, protectedSpans) && !Keywords.Contains(match.Value))
                        {
                            ApplyColor(absStart, match.Length, this.FunctionColor);
                        }
                    }
                }
            }
            finally
            {
                int safeStart = Math.Min(originalStart, this.TextLength);
                int safeLength = Math.Min(originalLength, Math.Max(0, this.TextLength - safeStart));

                this.SelectionStart = safeStart;
                this.SelectionLength = safeLength;

                if (this.IsHandleCreated)
                {
                    SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
                    this.Refresh();
                }

                this.isColorizing = false;
            }
        }

        private void ApplyColor(int start, int length, Color color)
        {
            if (start < 0 || length <= 0 || start + length > this.TextLength)
            {
                return;
            }

            this.SelectionStart = start;
            this.SelectionLength = length;
            this.SelectionColor = color;
        }

        private static bool IsInsideSpan(int index, List<(int Start, int End)> spans)
        {
            foreach (var span in spans)
            {
                if (index >= span.Start && index < span.End)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSyntaxRelevantChar(char c)
        {
            return char.IsLetterOrDigit(c)
                || c == '_'
                || c == '"'
                || c == '\\'
                || c == '/'
                || c == '('
                || c == ')';
        }
    }
}