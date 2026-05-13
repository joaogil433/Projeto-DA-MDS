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
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Database.SetInitializer(new IshoppingDbInitializer());

            using (var db = new IshoppingContext())
            {
                db.Database.Initialize(true);
            }

            Application.Run(new FormLogin());
        }
    }
}
