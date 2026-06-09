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

            context.TiposArtigo.AddRange(new List<TipoArtigo>
            {
                new TipoArtigo { Nome = "Alimentação" },
                new TipoArtigo { Nome = "Higiene" },
                new TipoArtigo { Nome = "Limpeza" }
            });

            context.SaveChanges();

            context.Artigos.AddRange(new List<Artigo>
            {
                new Artigo { Nome = "Arroz",            TipoArtigoId = 1 },
                new Artigo { Nome = "Massa",            TipoArtigoId = 1 },
                new Artigo { Nome = "Feijão",           TipoArtigoId = 1 },
                new Artigo { Nome = "Azeite",           TipoArtigoId = 1 },
                new Artigo { Nome = "Leite",            TipoArtigoId = 1 },
                new Artigo { Nome = "Pão",              TipoArtigoId = 1 },
                new Artigo { Nome = "Ovos",             TipoArtigoId = 1 },

                new Artigo { Nome = "Champô",           TipoArtigoId = 2 },
                new Artigo { Nome = "Pasta de Dentes",  TipoArtigoId = 2 },
                new Artigo { Nome = "Sabonete",         TipoArtigoId = 2 },
                new Artigo { Nome = "Papel Higiénico",  TipoArtigoId = 2 },

                new Artigo { Nome = "Detergente Roupa", TipoArtigoId = 3 },
                new Artigo { Nome = "Lava-Louça",       TipoArtigoId = 3 },
                new Artigo { Nome = "Desinfetante",     TipoArtigoId = 3 },
                new Artigo { Nome = "Esponjas",         TipoArtigoId = 3 },
            }); 

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
