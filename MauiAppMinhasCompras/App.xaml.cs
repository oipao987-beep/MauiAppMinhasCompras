using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        // Variável que guarda a conexão com o banco
        static SQLiteDatabaseHelper _db;

        // Propriedade para acessar o banco de dados
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                // Se o banco ainda não foi criado
                if (_db == null)
                {
                    // Define o caminho onde o banco será salvo
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3");

                    // Cria a conexão com o banco
                    _db = new SQLiteDatabaseHelper(path);
                }

                // Retorna a conexão
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            // Define a primeira tela do aplicativo
            MainPage = new AppShell();
        }
    }
}