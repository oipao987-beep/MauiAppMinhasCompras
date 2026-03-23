// Importa a classe que faz a conexão com o banco de dados SQLite
using MauiAppMinhasCompras.Helpers;
// Importa a classe Produto (modelo dos dados)
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System;

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
        CarregarProdutos();
    }

    private async void CarregarProdutos()
    {
        todosProdutos = await _db.GetAll();
        produtosFiltrados.Clear();
        foreach (var p in todosProdutos)
            produtosFiltrados.Add(p);

        lstProdutos.ItemsSource = produtosFiltrados;
        CalcularTotal(todosProdutos);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoBusca = e.NewTextValue?.ToLower().Trim() ?? "";
        produtosFiltrados.Clear();
        var resultados = todosProdutos.Where(p =>
             string.IsNullOrEmpty(textoBusca) ||
             p.Descricao.Contains(textoBusca, StringComparison.OrdinalIgnoreCase)
        ).ToList();

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
            var editarPage = new EditarProduto();
            editarPage.BindingContext = produto;   // ← passa o produto
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
            try
            {
                await _db.Delete(produto.Id);
                await DisplayAlert("Sucesso", "Produto excluído!", "OK");
                CarregarProdutos();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}