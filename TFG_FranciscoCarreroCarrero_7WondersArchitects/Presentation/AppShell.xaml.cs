namespace TFG_FranciscoCarreroCarrero_7WondersArchitects
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(GameBoardPage), typeof(GameBoardPage));
        }
    }
}
