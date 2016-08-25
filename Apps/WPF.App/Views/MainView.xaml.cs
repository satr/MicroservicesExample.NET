using WPF.App.ViewModels;

namespace WPF.App.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : IMainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
        }
    }
}
