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
            Database.SetInitializer(new IshoppingDbInitializer());
            using (var db = new IshoppingContext())
            {
                db.Database.Initialize(false);
            }



            // Abre o teu formulário diretamente!
            Application.Run(new Views.FormLogin());
        }
    }
}