using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditorProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _db;

    public EditorProduto()
    {
        InitializeComponent();
        _db = App.Db;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto p)
        {
            txtDescricao.Text = p.Descricao;
            txtQuantidade.Text = p.Quantidade.ToString();
            txtPreco.Text = p.Preco.ToString();
            pickerCategoria.SelectedItem = p.Categoria ?? "Outros";
        }
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                await DisplayAlert("Erro", "A descrição é obrigatória!", "OK");
                return;
            }

            if (BindingContext is not Produto produtoAtual)
                return;

            var produtoAtualizado = new Produto
            {
                Id = produtoAtual.Id,
                Descricao = txtDescricao.Text,
                Quantidade = int.Parse(txtQuantidade.Text),
                Preco = decimal.Parse(txtPreco.Text),
                Categoria = pickerCategoria.SelectedItem?.ToString() ?? "Outros"
            };

            await _db.Update(produtoAtualizado);
            await DisplayAlert("Sucesso!", "Produto atualizado com sucesso", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}