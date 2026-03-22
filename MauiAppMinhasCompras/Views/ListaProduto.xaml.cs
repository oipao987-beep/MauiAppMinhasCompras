// Importa a classe que faz a conexão com o banco de dados SQLite
using MauiAppMinhasCompras.Helpers;

// Importa a classe Produto (modelo dos dados)
using MauiAppMinhasCompras.Models;

using System.Collections.ObjectModel;   // necessário para ObservableCollection

namespace MauiAppMinhasCompras.Views;

// Página que mostra a lista de produtos
public partial class ListaProduto : ContentPage
{
    // Variável que guarda a conexão com o banco de dados
    private readonly SQLiteDatabaseHelper _db;

    // Novo - Lista completa vinda do SQLite 
    private List<Produto> todosProdutos = new List<Produto>();

    // Novo - ObservableCollection que a CollectionView usa 
    private ObservableCollection<Produto> produtosFiltrados = new ObservableCollection<Produto>();

    // Construtor da página (executa quando a tela abre)
    public ListaProduto()
    {
        InitializeComponent(); // Carrega os elementos visuais do XAML

        _db = App.Db; // Pega a conexão do banco criada no App.cs

        CarregarProdutos(); // Carrega os produtos do banco
    }

    // Novo - Método que busca todos os produtos no banco
    private async void CarregarProdutos()
    {
        todosProdutos = await _db.GetAll();

        produtosFiltrados.Clear();
        foreach (var p in todosProdutos)
            produtosFiltrados.Add(p);

        cvProdutos.ItemsSource = produtosFiltrados;   // Novo - liga à ObservableCollection
        CalcularTotal(todosProdutos);                 // Novo - total inicial = todos
    }

    // Novo - Evento TextChanged do SearchBar
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoBusca = e.NewTextValue?.ToLower().Trim() ?? "";

        produtosFiltrados.Clear();

        var resultados = todosProdutos.Where(p =>
            string.IsNullOrEmpty(textoBusca) ||
            p.Descricao.ToLower().Contains(textoBusca)
        ).ToList();

        foreach (var p in resultados)
            produtosFiltrados.Add(p);

        CalcularTotal(resultados);   // Novo - total agora reflete apenas os produtos filtrados
    }

    // Novo - Calcula o valor total (atualizado para aceitar lista filtrada)
    private void CalcularTotal(IEnumerable<Produto> produtos)
    {
        // Soma (Quantidade × Preço) de todos os produtos
        decimal total = produtos.Sum(p => p.Quantidade * p.Preco);

        lblTotal.Text = total.ToString("C"); // Mostra o total em formato de moeda (R$)
    }

    // Executa quando o botão "Novo Produto" é clicado
    private async void OnNovoClicked(object sender, EventArgs e)
    {
        // Abre a tela para cadastrar um novo produto
        await Navigation.PushAsync(new NovoProduto(_db));
    }

    // Executa sempre que a página aparece na tela
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        CarregarProdutos(); // Atualiza a lista de produtos
    }
}