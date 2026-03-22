using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); // Carrega os componentes da tela
        }

        private async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se algum campo está vazio
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    string.IsNullOrWhiteSpace(txtQuantidade.Text) ||
                    string.IsNullOrWhiteSpace(txtPreco.Text))
                {
                    await DisplayAlert("Atenção", "Preencha todos os campos!", "OK");
                    return;
                }

                // Cria um novo produto com os dados digitados
                var produto = new Produto
                {
                    Descricao = txtNome.Text,
                    Quantidade = Convert.ToInt32(txtQuantidade.Text),
                    Preco = Convert.ToDecimal(txtPreco.Text)
                };

                // Salva o produto no banco
                int id = await App.Db.Insert(produto);

                // Mostra mensagem de sucesso
                await DisplayAlert("Sucesso!", $"Produto salvo com ID: {id}", "OK");

                // Limpa os campos da tela
                txtNome.Text = string.Empty;
                txtQuantidade.Text = string.Empty;
                txtPreco.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // Mostra erro caso aconteça algum problema
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}