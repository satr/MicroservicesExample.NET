using App.WPF.ViewModels;

namespace App.WPF.Views
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
