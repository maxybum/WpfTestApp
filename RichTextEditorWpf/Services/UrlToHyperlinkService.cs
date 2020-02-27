using System.Windows.Controls;
using System.Windows.Documents;
using System;
using System.Text.RegularExpressions;

namespace RichTextEditorWpf.Services
{
    public class UrlToHyperlinkService : IUrlToHyperlinkService
    {
        private static readonly Regex UrlRegex = new Regex(@"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void UrlToHyperlinkCommand(RichTextBox mainRTB)
        {
            TextRange text = new TextRange(mainRTB.Document.ContentStart, mainRTB.Document.ContentEnd);
            TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
            while (current != null)
            {
                var textInRun = current.GetTextInRun(LogicalDirection.Forward);

                var match = UrlRegex.Match(textInRun).Value;

                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    if (!string.IsNullOrWhiteSpace(match))
                    {
                        int index = textInRun.IndexOf(match);
                        if (index != -1)
                        {
                            var selectionStart = current.GetPositionAtOffset(index);
                            var selectionEnd = selectionStart.GetPositionAtOffset(match.Length);
                            var selection = new TextRange(selectionStart, selectionEnd);

                            try
                            {
                                var hlink = new Hyperlink(selectionStart, selectionEnd);

                                hlink.NavigateUri = new Uri(match);
                            }
                            catch (Exception ex)
                            {
                                //TODO: implement Logger
                            }
                        }
                    }

                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }

    public interface IUrlToHyperlinkService
    {
        void UrlToHyperlinkCommand(RichTextBox mainRTB);
    }
}
