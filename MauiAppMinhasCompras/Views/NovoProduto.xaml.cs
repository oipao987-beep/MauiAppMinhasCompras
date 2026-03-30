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
            Quantidade = int.TryParse(txtQuantidade.Text, out int q) ? q : 0,
            Preco = decimal.TryParse(txtPreco.Text, out decimal preco) ? preco : 0,

            // !SALVA A CATEGORIA (Desafio 1)!
            Categoria = pickerCategoria.SelectedItem?.ToString() ?? "Outros"
        };

        await _db.Insert(produto);
        await Navigation.PopAsync();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}