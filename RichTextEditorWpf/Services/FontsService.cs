using System.Windows.Controls;
using System.Windows.Documents;

namespace RichTextEditorWpf.Services
{
    public class FontsService : IFontsService
    {
        public FontsService() { }
        public void ChangeFontFamilyCommand(ComboBox cb, RichTextBox rtb)
        {
            if (cb.SelectedItem != null)
                rtb.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cb.SelectedItem);
        }

        public void ChangeFontSizeCommand(ComboBox cb, RichTextBox rtb)
        {
            if (double.TryParse(cb.Text, out double result))
                rtb.Selection.ApplyPropertyValue(Inline.FontSizeProperty, result);
        }
    }

    public interface IFontsService
    {
        void ChangeFontFamilyCommand(ComboBox cb, RichTextBox rtb);
        void ChangeFontSizeCommand(ComboBox cb, RichTextBox rtb);
    }
}
