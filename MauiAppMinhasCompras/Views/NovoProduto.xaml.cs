// Importa a classe que faz o acesso ao banco de dados SQLite
using MauiAppMinhasCompras.Helpers;

// Importa o modelo Produto (estrutura dos dados do produto)
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

// Página usada para cadastrar um novo produto
public partial class NovoProduto : ContentPage
{
    // Variável que guarda a conexão com o banco de dados
    private readonly SQLiteDatabaseHelper _db;

    // Construtor da página
    // Recebe a conexão do banco vinda da página anterior
    public NovoProduto(SQLiteDatabaseHelper db)
    {
        InitializeComponent(); // Carrega os elementos visuais do XAML
        _db = db; // Armazena a conexão com o banco
    }

    // Método executado quando o botão "Salvar" é clicado
    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        // Verifica se a descrição foi preenchida
        if (string.IsNullOrWhiteSpace(txtDescricao.Text))
        {
            // Mostra um alerta caso esteja vazia
            await DisplayAlert("Atenção", "Preencha a descrição!", "OK");
            return; // Interrompe o salvamento
        }

        // Cria um novo objeto Produto com os dados digitados
        var produto = new Produto
        {
            Descricao = txtDescricao.Text, // Texto digitado na descrição

            // Tenta converter a quantidade para número inteiro
            // Se não conseguir converter, salva como 0
            Quantidade = int.TryParse(txtQuantidade.Text, out int q) ? q : 0,

            // Tenta converter o preço para decimal
            // Se não conseguir converter, salva como 0
            Preco = decimal.TryParse(txtPreco.Text, out decimal preco) ? preco : 0
        };

        // Salva o produto no banco de dados
        await _db.Insert(produto);

        // Fecha a tela atual e volta para a tela anterior
        await Navigation.PopAsync();
    }

    // Método executado quando o botão "Cancelar" é clicado
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Apenas volta para a tela anterior sem salvar
        await Navigation.PopAsync();
    }
}