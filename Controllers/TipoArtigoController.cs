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
        // Devolve todos os tipos de artigo ordenados por nome
        public List<TipoArtigo> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.TiposArtigo
                        .OrderBy(t => t.Nome)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter tipos de artigo: " + ex.Message);
            }
        }

        // Devolve um tipo de artigo pelo Id
        public TipoArtigo GetById(int id)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.TiposArtigo.FirstOrDefault(t => t.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter tipo de artigo: " + ex.Message);
            }
        }

        // Cria um novo tipo de artigo. Devolve true se correu bem
        public bool Add(string nome, out string mensagem)
        {
            mensagem = "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do tipo de artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    bool existe = db.TiposArtigo.Any(t =>
                        t.Nome.ToLower() == nome.Trim().ToLower());

                    if (existe)
                    {
                        mensagem = "Já existe um tipo de artigo com este nome.";
                        return false;
                    }

                    TipoArtigo novoTipo = new TipoArtigo();
                    novoTipo.Nome = nome.Trim();

                    db.TiposArtigo.Add(novoTipo);
                    db.SaveChanges();

                    mensagem = "Tipo de artigo criado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao criar tipo de artigo: " + ex.Message;
                return false;
            }
        }

        // Atualiza o nome de um tipo de artigo existente
        public bool Update(int id, string nome, out string mensagem)
        {
            mensagem = "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do tipo de artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    TipoArtigo tipo = db.TiposArtigo.Find(id);

                    if (tipo == null)
                    {
                        mensagem = "Tipo de artigo não encontrado.";
                        return false;
                    }

                    // Verifica duplicado mas exclui o próprio tipo que está a ser editado
                    bool duplicado = db.TiposArtigo.Any(t =>
                        t.Nome.ToLower() == nome.Trim().ToLower() &&
                        t.Id != id);

                    if (duplicado)
                    {
                        mensagem = "Já existe um tipo de artigo com este nome.";
                        return false;
                    }

                    tipo.Nome = nome.Trim();
                    db.SaveChanges();

                    mensagem = "Tipo de artigo atualizado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar tipo de artigo: " + ex.Message;
                return false;
            }
        }

        // Elimina um tipo de artigo. Protege contra eliminação se tiver artigos associados
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    TipoArtigo tipo = db.TiposArtigo.Find(id);

                    if (tipo == null)
                    {
                        mensagem = "Tipo de artigo não encontrado.";
                        return false;
                    }

                    bool temArtigos = db.Artigos.Any(a => a.TipoArtigoId == id);

                    if (temArtigos)
                    {
                        mensagem = "Não é possível eliminar: existem artigos associados a este tipo.";
                        return false;
                    }

                    db.TiposArtigo.Remove(tipo);
                    db.SaveChanges();

                    mensagem = "Tipo de artigo eliminado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao eliminar tipo de artigo: " + ex.Message;
                return false;
            }
        }
    }
}
