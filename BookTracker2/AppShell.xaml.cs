namespace BookTracker2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("BookDetailPage", typeof(Views.BookDetailPage));
        }
    }
}
