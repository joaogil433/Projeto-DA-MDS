// TipoArtigoController.cs
// Responsabilidade: lógica de negócio para a gestão de tipos de artigo (CRUD de Tipos)
// Regras principais:
//   - O nome do tipo tem de ser único
//   - Não é possível eliminar um tipo que tenha artigos associados

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
        // ── LEITURA ───────────────────────────────────────────────────────────

        // Devolve todos os tipos de artigo ordenados por nome
        public List<TipoArtigo> GetAll()
        {
            using (var db = new IshoppingContext())
            {
                return db.TiposArtigo.OrderBy(t => t.Nome).ToList();
            }
        }

        // Devolve um tipo de artigo pelo Id — usado para preencher o formulário de edição
        public TipoArtigo GetById(int id)
        {
            using (var db = new IshoppingContext())
            {
                return db.TiposArtigo.Find(id);
            }
        }

        // ── ESCRITA ───────────────────────────────────────────────────────────

        // Cria um novo tipo de artigo
        // Regra: o nome tem de ser único (case-insensitive)
        // Devolve false se o nome estiver vazio ou já existir
        public bool Add(string nome)
        {
            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome)) return false;

            using (var db = new IshoppingContext())
            {
                // Verifica se já existe um tipo com o mesmo nome (case-insensitive)
                bool existe = db.TiposArtigo.Any(t =>
                    t.Nome.ToLower() == nome.Trim().ToLower());

                if (existe) return false;

                // Cria e guarda o novo tipo
                db.TiposArtigo.Add(new TipoArtigo { Nome = nome.Trim() });

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // Atualiza o nome de um tipo de artigo existente
        // Verifica duplicado excluindo o próprio tipo que está a ser editado
        // Devolve false se o nome estiver vazio, for duplicado ou o tipo não existir
        public bool Update(int id, string nome)
        {
            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome)) return false;

            using (var db = new IshoppingContext())
            {
                // Vai à BD buscar o tipo pelo Id
                TipoArtigo tipo = db.TiposArtigo.Find(id);
                if (tipo == null) return false;

                // Verifica duplicado mas exclui o próprio tipo que está a ser editado
                bool duplicado = db.TiposArtigo.Any(t =>
                    t.Nome.ToLower() == nome.Trim().ToLower() && t.Id != id);

                if (duplicado) return false;

                // Atualiza o nome
                tipo.Nome = nome.Trim();

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // Elimina um tipo de artigo pelo Id
        // Proteção: não permite eliminar se existirem artigos associados a este tipo
        // Devolve false se o tipo não existir, tiver artigos associados, ou ocorrer erro
        public bool Delete(int id)
        {
            using (var db = new IshoppingContext())
            {
                // Vai à BD buscar o tipo pelo Id
                TipoArtigo tipo = db.TiposArtigo.Find(id);
                if (tipo == null) return false;

                // Proteção: não pode eliminar se existirem artigos deste tipo
                bool temArtigos = db.Artigos.Any(a => a.TipoArtigoId == id);
                if (temArtigos) return false;

                db.TiposArtigo.Remove(tipo);

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}