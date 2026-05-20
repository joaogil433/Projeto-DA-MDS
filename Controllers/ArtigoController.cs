using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class ArtigoController
    {
        // Devolve todos os artigos com o Tipo carregado, ordenados por Tipo e depois por Nome
        public List<Artigo> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Artigos
                        .Include(a => a.Tipo)
                        .OrderBy(a => a.Tipo.Nome)
                        .ThenBy(a => a.Nome)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter artigos: " + ex.Message);
            }
        }

        // Devolve os artigos de um tipo específico — usado para filtrar o ComboBox de artigos
        public List<Artigo> GetByTipo(int tipoArtigoId)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Artigos
                        .Where(a => a.TipoArtigoId == tipoArtigoId)
                        .Include(a => a.Tipo)
                        .OrderBy(a => a.Nome)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao filtrar artigos por tipo: " + ex.Message);
            }
        }

        // Devolve um artigo pelo Id — usado para preencher o formulário de edição
        public Artigo GetById(int id)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Artigos
                        .Include(a => a.Tipo)
                        .FirstOrDefault(a => a.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter artigo: " + ex.Message);
            }
        }

        // Cria um novo artigo. Devolve true se correu bem, false se houve erro de validação
        public bool Add(string nome, int tipoArtigoId, out string mensagem)
        {
            mensagem = "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    bool existe = db.Artigos.Any(a =>
                        a.Nome.ToLower() == nome.Trim().ToLower() &&
                        a.TipoArtigoId == tipoArtigoId);

                    if (existe)
                    {
                        mensagem = "Já existe um artigo com este nome neste tipo.";
                        return false;
                    }

                    Artigo novoArtigo = new Artigo();
                    novoArtigo.Nome = nome.Trim();
                    novoArtigo.TipoArtigoId = tipoArtigoId;

                    db.Artigos.Add(novoArtigo);
                    db.SaveChanges();

                    mensagem = "Artigo criado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao criar artigo: " + ex.Message;
                return false;
            }
        }

        // Atualiza o nome e o tipo de um artigo existente
        public bool Update(int id, string nome, int tipoArtigoId, out string mensagem)
        {
            mensagem = "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    Artigo artigo = db.Artigos.Find(id);

                    if (artigo == null)
                    {
                        mensagem = "Artigo não encontrado.";
                        return false;
                    }

                    // Verifica duplicado mas exclui o próprio artigo que está a ser editado
                    bool duplicado = db.Artigos.Any(a =>
                        a.Nome.ToLower() == nome.Trim().ToLower() &&
                        a.TipoArtigoId == tipoArtigoId &&
                        a.Id != id);

                    if (duplicado)
                    {
                        mensagem = "Já existe um artigo com este nome neste tipo.";
                        return false;
                    }

                    artigo.Nome = nome.Trim();
                    artigo.TipoArtigoId = tipoArtigoId;
                    db.SaveChanges();

                    mensagem = "Artigo atualizado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar artigo: " + ex.Message;
                return false;
            }
        }

        // Elimina um artigo. Protege contra eliminação se estiver em uso em compras
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    Artigo artigo = db.Artigos.Find(id);

                    if (artigo == null)
                    {
                        mensagem = "Artigo não encontrado.";
                        return false;
                    }

                    bool emUso = db.ItensCompra.Any(i => i.ArtigoId == id);

                    if (emUso)
                    {
                        mensagem = "Não é possível eliminar: este artigo está associado a compras.";
                        return false;
                    }

                    db.Artigos.Remove(artigo);
                    db.SaveChanges();

                    mensagem = "Artigo eliminado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao eliminar artigo: " + ex.Message;
                return false;
            }
        }
    }
}
