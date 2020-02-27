using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using RichTextEditorWpf.Services;
using Unity;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;

namespace RichTextEditorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IImageService _imageService;
        private IFontsService _fontService;
        private ISaveService _saveService;
        private ILoadService _loadService;
        private IPrintService _printService;
        private IUrlToHyperlinkService _urlToHyperlinkService;

        public MainWindow()
        {
            InitializationTemplate();
        }

        private void ChangeAllUrlToHyperlink(object sender, RoutedEventArgs e)
        {
            _urlToHyperlinkService.UrlToHyperlinkCommand(mainRTB);
        }

        public void InitializationTemplate()
        {
            InitializeComponent();

            RegisterServices();

            InitiazeDefaultValues();
        }

        private void RegisterServices()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IImageService, ImageService>();
            container.RegisterType<IFontsService, FontsService>();
            container.RegisterType<ISaveService, SaveService>();
            container.RegisterType<ILoadService, LoadService>();
            container.RegisterType<IPrintService, PrintService>();
            container.RegisterType<IUrlToHyperlinkService, UrlToHyperlinkService>();

            _imageService = container.Resolve<ImageService>();
            _fontService = container.Resolve<FontsService>();
            _saveService = container.Resolve<SaveService>();
            _loadService = container.Resolve<LoadService>();
            _printService = container.Resolve<PrintService>();
            _urlToHyperlinkService = container.Resolve<UrlToHyperlinkService>();
        }

        private void InitiazeDefaultValues()
        {
            var fontList = Fonts.SystemFontFamilies;
            cmbFontFamily.ItemsSource = fontList.OrderBy(f => f.Source);
            cmbFontFamily.SelectedItem = fontList.First();

            var fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontSize.ItemsSource = fontSizes;
            cmbFontSize.SelectedItem = fontSizes.First();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SelectionChangedCommand();
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            _imageService.AddImgCommand(mainRTB);
        }

        private void ChangeFontFamily(object sender, SelectionChangedEventArgs e)
        {
            _fontService.ChangeFontFamilyCommand(cmbFontFamily, mainRTB);
        }

        private void ChangeFontSize(object sender, TextChangedEventArgs e)
        {
            _fontService.ChangeFontSizeCommand(cmbFontSize, mainRTB);
        }

        void PrintRTBContent(object sender, RoutedEventArgs args)
        {
            _printService.PrintCommand(mainRTB);
        }

        void SaveRTBContent(object sender, RoutedEventArgs args)
        {
            _saveService.SaveCommand(mainRTB);
        }

        void LoadFile(object sender, RoutedEventArgs args)
        {
            _loadService.LoadCommand(mainRTB);
        }

        private void SelectionChangedCommand()
        {
            object temp = mainRTB.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = mainRTB.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = mainRTB.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = mainRTB.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = mainRTB.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();

            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            UpdateToggleButtonState();
            UpdateSelectionListType();
        }

        private void UpdateSelectionListType()
        {
            Paragraph startParagraph = mainRTB.Selection.Start.Paragraph;
            Paragraph endParagraph = mainRTB.Selection.End.Paragraph;
            if (startParagraph != null && endParagraph != null && (startParagraph.Parent is ListItem) && (endParagraph.Parent is ListItem) && object.ReferenceEquals(((ListItem)startParagraph.Parent).List, ((ListItem)endParagraph.Parent).List))
            {
                TextMarkerStyle markerStyle = ((ListItem)startParagraph.Parent).List.MarkerStyle;
                if (markerStyle == TextMarkerStyle.Disc) //bullets  
                {
                    _btnBullets.IsChecked = true;
                }
                else if (markerStyle == TextMarkerStyle.Decimal) //number  
                {
                    _btnNumbers.IsChecked = true;
                }
            }
            else
            {
                _btnBullets.IsChecked = false;
                _btnNumbers.IsChecked = false;
            }
        }

        void UpdateItemCheckedState(ToggleButton button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = mainRTB.Selection.GetPropertyValue(formattingProperty);
            button.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }

        private void UpdateToggleButtonState()
        {
            UpdateItemCheckedState(_btnAlignLeft, Paragraph.TextAlignmentProperty, TextAlignment.Left);
            UpdateItemCheckedState(_btnAlignCenter, Paragraph.TextAlignmentProperty, TextAlignment.Center);
            UpdateItemCheckedState(_btnAlignRight, Paragraph.TextAlignmentProperty, TextAlignment.Right);
            UpdateItemCheckedState(_btnAlignJustify, Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }
    }
}
