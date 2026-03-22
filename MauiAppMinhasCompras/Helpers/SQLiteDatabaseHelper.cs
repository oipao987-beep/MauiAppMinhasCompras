// Importa o modelo Produto (estrutura da tabela no banco)
using MauiAppMinhasCompras.Models;

// Importa a biblioteca SQLite usada para trabalhar com banco de dados local
using SQLite;

// Importa ferramentas para trabalhar com listas
using System.Collections.Generic;

// Importa suporte a métodos assíncronos (async/await)
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Helpers
{
    // Classe responsável por fazer todas as operações no banco de dados
    public class SQLiteDatabaseHelper
    {
        // Conexão assíncrona com o banco SQLite
        readonly SQLiteAsyncConnection _conn;

        // Construtor da classe
        // Recebe o caminho onde o banco de dados será criado ou acessado
        public SQLiteDatabaseHelper(string path)
        {
            // Cria a conexão com o banco
            _conn = new SQLiteAsyncConnection(path);

            // Cria a tabela Produto caso ela ainda não exista
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método para inserir um novo produto no banco
        public Task<int> Insert(Produto p) => _conn.InsertAsync(p);

        // Método para atualizar um produto já existente
        public Task<int> Update(Produto p)
        {
            // Comando SQL que atualiza os dados do produto
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            // Executa o comando passando os novos valores
            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        // Método para deletar um produto pelo Id
        public Task<int> Delete(int id) =>
            _conn.Table<Produto>().DeleteAsync(i => i.Id == id);

        // Método para buscar todos os produtos da tabela
        public Task<List<Produto>> GetAll() =>
            _conn.Table<Produto>().ToListAsync();

        // Método para pesquisar produtos pela descrição
        public Task<List<Produto>> Search(string q)
        {
            // Comando SQL que procura produtos que contenham o texto digitado
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";

            // Executa a consulta e retorna os resultados
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}