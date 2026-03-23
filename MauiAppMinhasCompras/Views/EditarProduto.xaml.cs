using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _db;

    public EditarProduto()
    {
        InitializeComponent();
        _db = App.Db;                    // usa a mesma conexão que você já tem
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Preenche os campos com os dados atuais do produto
        if (BindingContext is Produto p)
        {
            txtDescricao.Text = p.Descricao;
            txtQuantidade.Text = p.Quantidade.ToString();
            txtPreco.Text = p.Preco.ToString();
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
            if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                await DisplayAlert("Erro", "A quantidade é obrigatória!", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                await DisplayAlert("Erro", "O preço é obrigatório!", "OK");
                return;
            }

            var produtoAtual = BindingContext as Produto;

            var produtoAtualizado = new Produto
            {
                Id = produtoAtual.Id,
                Descricao = txtDescricao.Text,
                Quantidade = int.Parse(txtQuantidade.Text),   
                Preco = decimal.Parse(txtPreco.Text)
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