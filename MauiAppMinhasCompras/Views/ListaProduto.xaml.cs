using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private SQLiteDatabaseHelper _db;

    public ListaProduto()
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fichario.db3");
        _db = new SQLiteDatabaseHelper(dbPath);

        CarregarProdutos();
    }

    private async void CarregarProdutos()
    {
        cvProdutos.ItemsSource = await _db.GetAll();
    }

    private async void OnNovoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto(_db));
    }
}