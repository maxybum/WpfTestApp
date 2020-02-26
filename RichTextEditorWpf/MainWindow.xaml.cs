using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using RichTextEditorWpf.Services;
using Unity;

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
        
        public MainWindow()
        {
            InitializationTemplate();
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

            _imageService = container.Resolve<ImageService>();
            _fontService = container.Resolve<FontsService>();
            _saveService = container.Resolve<SaveService>();
            _loadService = container.Resolve<LoadService>();
            _printService = container.Resolve<PrintService>();
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
    }
}
