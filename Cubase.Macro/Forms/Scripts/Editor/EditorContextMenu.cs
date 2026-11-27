using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Forms.Scripts.Editor
{
    public class EditorContextMenu : ContextMenuStrip
    {
        public EditorContextMenu(ScriptEditor scriptEditor, CubaseScriptCommandCollection functions, CubaseKeyCommandCollection midiFunctions) : base()
        {
            this.Items.Add(new FunctionMenu(functions, scriptEditor));
            this.Items.Add(new MidiFunctionMenu(midiFunctions, scriptEditor));
            this.Items.Add(new CopyMenu(scriptEditor));
            this.Items.Add(new PasteMenu(scriptEditor));
            this.Items.Add(new CutMenu(scriptEditor));
        }
    }

    public class CopyMenu : ToolStripMenuItem
    {
        private ScriptEditor Editor;

        public CopyMenu(ScriptEditor editor)
        {
            this.Editor = editor;
            this.Text = "Copy";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Editor.Copy();
        }
    }

    public class PasteMenu : ToolStripMenuItem
    {
        private ScriptEditor Editor;

        public PasteMenu(ScriptEditor editor)
        {
            this.Editor = editor;
            this.Text = "Paste";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Editor.Paste();
        }
    }

    public class CutMenu : ToolStripMenuItem
    {
        private ScriptEditor Editor;

        public CutMenu(ScriptEditor editor)
        {
            this.Editor = editor;
            this.Text = "Cut";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Editor.Cut();
        }
    }

    public class FunctionCommand : ToolStripMenuItem
    {
        private readonly CubaseScriptCommand Command;
        private readonly ScriptEditor Editor;
        private readonly ToolTip toolTip;

        public FunctionCommand(CubaseScriptCommand command, ScriptEditor editor)
        {
            this.Command = command;
            this.Editor = editor;
            this.Text = command.Name;

            this.toolTip = new ToolTip
            {
                InitialDelay = 200,
                ReshowDelay = 100
            };
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Editor.InsertText(this.Command.Name);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (string.IsNullOrWhiteSpace(this.Command?.Description))
            {
                return;
            }

            var parent = this.GetCurrentParent();
            if (parent == null)
            {
                return;
            }

            // Place just to the right of the menu item, aligned vertically with it
            int x = this.Bounds.Right + 6;
            int y = this.Bounds.Top;

            this.toolTip.Show(this.Command.Description, parent, x, y);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            var parent = this.GetCurrentParent();
            if (parent != null)
            {
                this.toolTip.Hide(parent);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.toolTip.Dispose();
            }

            base.Dispose(disposing);
        }
    }


    public class FunctionMenu : ToolStripMenuItem
    {
        private CubaseScriptCommandCollection Functions;

        private ScriptEditor Editor;

        public FunctionMenu(CubaseScriptCommandCollection functions, ScriptEditor editor)
        {
            this.Functions = functions;
            this.Editor = editor;
            this.Text = "Functions";
            foreach (var function in functions)
            {
                this.DropDown.Items.Add(new FunctionCommand(function, editor));
            }

        }
    }

    public class MidiFunctionMenu : ToolStripMenuItem
    {
        private CubaseKeyCommandCollection MidiFunctions;

        private ScriptEditor Editor;

        public MidiFunctionMenu(CubaseKeyCommandCollection functions, ScriptEditor editor)
        {
            this.MidiFunctions = functions;
            this.Editor = editor;
            this.Text = "Midi Functions";
            foreach (var func in functions)
            {
                this.DropDown.Items.Add(new MidiFunction(func, editor));
            }
        }
    }

    public class MidiFunction : ToolStripMenuItem
    {
        private CubaseKeyCommand Command;

        private ScriptEditor Editor;

        public MidiFunction(CubaseKeyCommand command, ScriptEditor editor)
        {
            this.Command = command;
            this.Editor = editor;
            this.Text = command.Name;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Editor.InsertText(this.Command.Name);
        }
    }
}
