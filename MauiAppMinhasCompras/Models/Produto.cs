using SQLite;

namespace MauiAppMinhasCompras.Models
{
    [Table("Produto")]
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }

        //  !NOVO DO DESAFIO 1!
        public string Categoria { get; set; } = string.Empty;
    }
}