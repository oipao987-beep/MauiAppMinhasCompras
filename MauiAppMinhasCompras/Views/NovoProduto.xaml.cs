using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _db;

    public NovoProduto(SQLiteDatabaseHelper db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtDescricao.Text))
        {
            await DisplayAlert("Atenção", "Preencha a descrição!", "OK");
            return;
        }

        var produto = new Produto  
        {
            Descricao = txtDescricao.Text,
            Quantidade = double.TryParse(txtQuantidade.Text, out double q) ? q : 0,
            Preco = double.TryParse(txtPreco.Text, out double preco) ? preco : 0
        };

        await _db.Insert(produto);

        await Navigation.PopAsync();
    }
    

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}