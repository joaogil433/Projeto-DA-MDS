using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projeto_DA_MDS.Views;
using System.Data.Entity;

namespace Projeto_DA_MDS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Inicializa a BD (podes deixar isto se for preciso criar a BD)
            Database.SetInitializer(new IshoppingDbInitializer());

            // 2. Vamos buscar a lista aberta DIRETO aqui no arranque para testar
            using (var db = new IshoppingContext())
            {
                db.Database.Initialize(true); // Garante que a BD está pronta

                var lista = db.ListasCompras.FirstOrDefault(l => l.Estado == "Aberta");

                if (lista != null)
                {
                    // Se encontrar a lista, abre DIRETO o teu FormModoCompra!
                    Application.Run(new FormModoCompra(lista));
                }
                else
                {
                    // Se não encontrar, avisa-te logo e abre o Form1 antigo para não dar erro
                    MessageBox.Show("Aviso de Teste: Não foi encontrada nenhuma lista 'Aberta' na BD!");
                    Application.Run(new Form1());
                }
            }
        }
    }
}