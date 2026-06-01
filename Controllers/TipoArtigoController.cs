using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Controllers
{
    internal class TipoArtigoController
    {
        public List<TipoArtigo> GetAll()
        {
            using (var db = new IshoppingContext())
            {
                return db.TiposArtigo.OrderBy(t => t.Nome).ToList();
            }
        }

        public TipoArtigo GetById(int id)
        {
            using (var db = new IshoppingContext())
            {
                return db.TiposArtigo.Find(id);
            }
        }

        public bool Add(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return false;

            using (var db = new IshoppingContext())
            {
                bool existe = db.TiposArtigo.Any(t =>
                    t.Nome.ToLower() == nome.Trim().ToLower());

                if (existe) return false;

                db.TiposArtigo.Add(new TipoArtigo { Nome = nome.Trim() });
                db.SaveChanges();
                return true;
            }
        }

        public bool Update(int id, string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return false;

            using (var db = new IshoppingContext())
            {
                TipoArtigo tipo = db.TiposArtigo.Find(id);
                if (tipo == null) return false;

                bool duplicado = db.TiposArtigo.Any(t =>
                    t.Nome.ToLower() == nome.Trim().ToLower() && t.Id != id);

                if (duplicado) return false;

                tipo.Nome = nome.Trim();
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new IshoppingContext())
            {
                TipoArtigo tipo = db.TiposArtigo.Find(id);
                if (tipo == null) return false;

                bool temArtigos = db.Artigos.Any(a => a.TipoArtigoId == id);
                if (temArtigos) return false;

                db.TiposArtigo.Remove(tipo);
                db.SaveChanges();
                return true;
            }
        }
    }
}
