using System;
using System.Data.Entity;
using System.Windows.Forms;
using System.Linq;

namespace Projeto_DA_MDS
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicializa apenas o contexto básico
            Database.SetInitializer(new CreateDatabaseIfNotExists<IshoppingContext>());
            using (var db = new IshoppingContext())
            {
                db.Database.Initialize(false);
            }

            // Criamos um objeto de lista em memória apenas para o teu construtor aceitar
            var listaTeste = new Models.ListaCompra
            {
                Id = 1,
                Estado = "Aberta"
            };

            // Abre o teu formulário diretamente!
            Application.Run(new Views.FormModoCompra(listaTeste));
        }
    }
}