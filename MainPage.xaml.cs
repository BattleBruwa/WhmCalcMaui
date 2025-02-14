using WhmCalcMaui.Services;

namespace WhmCalcMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public ModListService ModList { get; set; }

        public MainPage(ModListService modList)
        {
            ModList = modList;
            InitializeComponent();
            BindingContext = ModList;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
