using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RichTextEditorWpf.Services
{
    public class PrintService : IPrintService
    {
        public void PrintCommand(RichTextBox mainRTB)
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))
            {
                //use either one of the below      
                pd.PrintVisual(mainRTB as Visual, "printing as visual");
                pd.PrintDocument((((IDocumentPaginatorSource)mainRTB.Document).DocumentPaginator), "printing as paginator");
            }
        }
    }

    public interface IPrintService
    {
        void PrintCommand(RichTextBox mainRTB);
    }
}
