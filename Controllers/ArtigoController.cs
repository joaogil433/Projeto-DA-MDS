// ArtigoController.cs
// Responsabilidade: lógica de negócio para a gestão de artigos (CRUD de Artigos)
// Regras principais:
//   - O nome do artigo tem de ser único dentro do mesmo tipo
//   - Não é possível eliminar artigos que estejam associados a itens de compra

using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class ArtigoController
    {
        // ── LEITURA ───────────────────────────────────────────────────────────

        // Devolve todos os artigos com o Tipo carregado
        // Ordenados primeiro por nome do tipo, depois por nome do artigo
        public List<Artigo> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Artigos
                        .Include(a => a.Tipo)           // carrega a propriedade de navegação Tipo
                        .OrderBy(a => a.Tipo.Nome)      // ordena por tipo
                        .ThenBy(a => a.Nome)            // depois por nome do artigo
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter artigos: " + ex.Message);
            }
        }

        // Devolve os artigos de um tipo específico
        // Usado para filtrar o ComboBox de artigos no formulário de criação de compras
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

        // ── ESCRITA ───────────────────────────────────────────────────────────

        // Cria um novo artigo
        // Regra: o nome tem de ser único dentro do mesmo tipo de artigo
        // Devolve true se criado com sucesso; false com mensagem de erro em caso de falha
        public bool Add(string nome, int tipoArtigoId, out string mensagem)
        {
            mensagem = "";

            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Verifica se já existe um artigo com o mesmo nome neste tipo (case-insensitive)
                    bool existe = db.Artigos.Any(a =>
                        a.Nome.ToLower() == nome.Trim().ToLower() &&
                        a.TipoArtigoId == tipoArtigoId);

                    if (existe)
                    {
                        mensagem = "Já existe um artigo com este nome neste tipo.";
                        return false;
                    }

                    // Cria e guarda o novo artigo
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
        // Verifica duplicado excluindo o próprio artigo que está a ser editado
        public bool Update(int id, string nome, int tipoArtigoId, out string mensagem)
        {
            mensagem = "";

            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome do artigo não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Vai à BD buscar o artigo pelo Id
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

                    // Atualiza os campos
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

        // Elimina um artigo pelo Id
        // Proteção: não permite eliminar se o artigo estiver associado a itens de compra
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Vai à BD buscar o artigo pelo Id
                    Artigo artigo = db.Artigos.Find(id);

                    if (artigo == null)
                    {
                        mensagem = "Artigo não encontrado.";
                        return false;
                    }

                    // Verifica se o artigo está em uso em algum item de compra
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