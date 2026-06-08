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
                try
                {
                    db.Database.Initialize(false);
                }
                catch (Exception ex){
                    MessageBox.Show("Erro ao ligar à base de dados:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }



            // Abre o teu formulário diretamente!
            Application.Run(new Views.FormLogin());
        }
    }
}