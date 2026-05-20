using Projeto_DA_MDS.Helpers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS
{
    public class IshoppingDbInitializer: DropCreateDatabaseIfModelChanges<IshoppingContext>
    //public class IshoppingDbInitializer : DropCreateDatabaseAlways<IshoppingContext>
    {
        protected override void Seed(IshoppingContext context)
        {
            context.Utilizadores.AddRange(new List<Utilizador>
            {
                new Utilizador { Nome = "João", Username = "admin1", Password = HashHelper.HashPassword("1234") },
                new Utilizador { Nome = "Duarte",    Username = "admin2",  Password = HashHelper.HashPassword("1234") },
                new Utilizador { Nome = "Rafa",  Username = "admin3", Password = HashHelper.HashPassword("1234") }
            });

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
