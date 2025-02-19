using WhmCalcMaui.ViewModels;

namespace WhmCalcMaui.Views;

public partial class MainView : ContentPage
{
    MainViewModel viewModel;

    public MainView(MainViewModel viewModel)
    {
        this.viewModel = viewModel;
        InitializeComponent();
        BindingContext = this.viewModel;
    }
}