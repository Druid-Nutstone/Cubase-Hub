using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    public class ConditionalKeyPanel : TableLayoutPanel
    {
        public ConditionalKeyPanel()
        {
            Dock = DockStyle.Top;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            AutoScroll = false;

            Font = new System.Drawing.Font(Font.FontFamily, 12, FontStyle.Regular);
        }

        public void AddCondition(CubaseCondition cubaseCondition,
                                 IServiceProvider serviceProvider)
        {
            var conditionalKeyControl = new ConditionalKeyControl(cubaseCondition, serviceProvider);
            this.AddControl(conditionalKeyControl);
        }

        public void Reset()
        {
            RowCount = 0;
            Controls.Clear();
        }

        private void AddControl(ConditionalKeyControl cntrl)
        {
            SuspendLayout();

            int row = RowCount;

            RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Controls.Add(cntrl, 0, row);

            RowCount++;

            ResumeLayout();
        }

    }
}
