using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _db;
    private List<Produto> todosProdutos = new List<Produto>();
    private ObservableCollection<Produto> produtosFiltrados = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        _db = App.Db;

        // Preenche o Picker com as opções
        pickerCategoriaFiltro.ItemsSource = new List<string>
        {
            "Todas", "Alimentos", "Higiene Pessoal", "Limpeza", "Bebidas", "Outros"
        };
        pickerCategoriaFiltro.SelectedIndex = 0; // Todas
    }

    private async void CarregarProdutos(string categoriaFiltro = "Todas")
    {
        todosProdutos = await _db.GetAll();

        var filtrados = todosProdutos;

        // Aplica filtro de categoria
        if (categoriaFiltro != "Todas")
            filtrados = filtrados.Where(p => p.Categoria == categoriaFiltro).ToList();

        produtosFiltrados.Clear();
        foreach (var p in filtrados)
            produtosFiltrados.Add(p);

        lstProdutos.ItemsSource = produtosFiltrados;
        CalcularTotal(filtrados);
    }

    private void OnFiltroCategoriaChanged(object sender, EventArgs e)
    {
        string filtro = pickerCategoriaFiltro.SelectedItem?.ToString() ?? "Todas";
        CarregarProdutos(filtro);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoBusca = e.NewTextValue?.ToLower().Trim() ?? "";
        string categoriaFiltro = pickerCategoriaFiltro.SelectedItem?.ToString() ?? "Todas";

        var resultados = todosProdutos.Where(p =>
            (string.IsNullOrEmpty(textoBusca) || p.Descricao.Contains(textoBusca, StringComparison.OrdinalIgnoreCase)) &&
            (categoriaFiltro == "Todas" || p.Categoria == categoriaFiltro)
        ).ToList();

        produtosFiltrados.Clear();
        foreach (var p in resultados)
            produtosFiltrados.Add(p);

        lstProdutos.ItemsSource = produtosFiltrados;
        CalcularTotal(resultados);
    }

    private void CalcularTotal(IEnumerable<Produto> produtos)
    {
        decimal total = produtos.Sum(p => p.Quantidade * p.Preco);
        lblTotal.Text = total.ToString("C");
    }

    private async void OnNovoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto(_db));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CarregarProdutos();
    }

    // CLIQUE NO PRODUTO QUE ABRE EDIÇÃO
    private async void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Produto produto)
        {
            var editarPage = new EditorProduto();   // ✅ CORRIGIDO AQUI
            editarPage.BindingContext = produto;
            await Navigation.PushAsync(editarPage);
        }
    }

    // BOTÃO DIREITO EXCLUIR
    private async void OnExcluirClicked(object sender, EventArgs e)
    {
        var menuItem = sender as MenuItem;
        var produto = menuItem?.BindingContext as Produto;
        if (produto == null) return;

        bool confirm = await DisplayAlert("Confirmação",
            $"Deseja realmente excluir '{produto.Descricao}'?", "Sim", "Não");

        if (confirm)
        {
            await _db.Delete(produto.Id);
            await DisplayAlert("Sucesso", "Produto excluído!", "OK");
            CarregarProdutos(pickerCategoriaFiltro.SelectedItem?.ToString() ?? "Todas");
        }
    }

    // !RELATÓRIO POR CATEGORIA (Desafio 1)!
    private async void OnRelatorioClicked(object sender, EventArgs e)
    {
        try
        {
            var relatorio = todosProdutos
                .Where(p => !string.IsNullOrEmpty(p.Categoria))
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(p => p.Quantidade * p.Preco)
                })
                .OrderByDescending(g => g.Total);

            string texto = "=== RELATÓRIO POR CATEGORIA ===\n\n";

            foreach (var item in relatorio)
            {
                texto += $"{item.Categoria}: {item.Total:C}\n";
            }

            if (!relatorio.Any())
                texto += "Nenhum produto cadastrado.";

            await DisplayAlert("Relatório de Gastos por Categoria", texto, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}