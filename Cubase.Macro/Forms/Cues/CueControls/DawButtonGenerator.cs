using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public static class DawButtonGenerator
    {
        // Professional Cubase-inspired Dark Theme Palette
        private static readonly Color BaseButtonColor = Color.FromArgb(45, 45, 45);
        private static readonly Color BorderColor = Color.FromArgb(30, 30, 30);

        public static Bitmap CreateMuteButton(bool isActive)
        {
            // Cubase uses a distinct Yellow/Muted Gold for Mute
            Color textColor = isActive ? Color.FromArgb(240, 180, 20) : Color.FromArgb(160, 160, 160);
            Color bgColor = isActive ? Color.FromArgb(70, 60, 20) : BaseButtonColor;
            return GenerateTextButton("M", textColor, bgColor);
        }

        public static Bitmap CreateSoloButton(bool isActive)
        {
            // Cubase uses a bright, high-contrast Green for Solo
            Color textColor = isActive ? Color.FromArgb(40, 220, 100) : Color.FromArgb(160, 160, 160);
            Color bgColor = isActive ? Color.FromArgb(20, 65, 35) : BaseButtonColor;
            return GenerateTextButton("S", textColor, bgColor);
        }

        public static Bitmap CreateRecordButton(bool isActive)
        {
            // Cubase uses a striking Dark Red for armed record state
            Bitmap bmp = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetupGraphics(g);
                Color bgColor = isActive ? Color.FromArgb(120, 20, 20) : BaseButtonColor;
                Color dotColor = isActive ? Color.FromArgb(255, 50, 50) : Color.FromArgb(180, 50, 50);

                // Draw Background Button Frame
                DrawButtonBase(g, bgColor);

                // Draw Record Circle
                using (Brush dotBrush = new SolidBrush(dotColor))
                {
                    // Centered 12x12 pixel record dot
                    g.FillEllipse(dotBrush, 9, 9, 12, 12);
                }
            }
            return bmp;
        }

        private static Bitmap GenerateTextButton(string text, Color textColor, Color bgColor)
        {
            Bitmap bmp = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetupGraphics(g);
                DrawButtonBase(g, bgColor);

                // Render crisp, bold UI typography aligned precisely
                using (Font font = new Font("Segoe UI", 11, FontStyle.Bold, GraphicsUnit.Point))
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    // Offset vertically by 1 pixel for perfect visual center alignment
                    g.DrawString(text, font, textBrush, new RectangleF(0, 1, 30, 30), sf);
                }
            }
            return bmp;
        }

        private static void SetupGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.Transparent); // Enables WinForms Alpha Transparency
        }

        private static void DrawButtonBase(Graphics g, Color bgColor)
        {
            // Fill 30x30 button bounds
            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, 0, 0, 29, 29);
            }
            // Draw outer UI border
            using (Pen borderPen = new Pen(BorderColor, 1))
            {
                g.DrawRectangle(borderPen, 0, 0, 29, 29);
            }
        }
    }

}
