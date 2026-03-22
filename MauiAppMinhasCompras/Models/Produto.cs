using SQLite;

namespace MauiAppMinhasCompras.Models
{
    // Classe que representa um produto no banco
    [Table("Produto")]
    public class Produto
    {
        // Id do produto
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Descrição
        public string Descricao { get; set; }

        // Quantidade
        public int Quantidade { get; set; }

        // Preço
        public decimal Preco { get; set; }
    }
}