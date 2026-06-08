// ListaCompraController.cs
// Responsabilidade: lógica de negócio para a gestão de listas de compras (US3, US4)
// Regras principais:
//   - Uma lista começa sempre com estado "Aberta"
//   - Listas fechadas não podem ser editadas nem eliminadas
//   - Não é possível adicionar o mesmo artigo duas vezes à mesma lista
//   - Regista sempre quem criou e quem alterou

using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class ListaCompraController
    {
        // ── LEITURA ───────────────────────────────────────────────────────────

        // Devolve todas as listas de compras ordenadas da mais recente para a mais antiga
        // Carrega os utilizadores que criaram e alteraram (para auditoria na View)
        public List<ListaCompra> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.ListasCompras
                        .Include(l => l.UtilizadorCriou)
                        .Include(l => l.UtilizadorAlterou)
                        .OrderByDescending(l => l.DataCriacao)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter listas de compras: " + ex.Message);
            }
        }

        // Devolve as listas filtradas pelo estado ("Aberta" ou "Fechada")
        public List<ListaCompra> GetByEstado(string estado)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.ListasCompras
                        .Where(l => l.Estado == estado)
                        .Include(l => l.UtilizadorCriou)
                        .Include(l => l.UtilizadorAlterou)
                        .OrderByDescending(l => l.DataCriacao)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao filtrar listas por estado: " + ex.Message);
            }
        }

        // Atalho para obter apenas as listas abertas — usado no FormPrincipal
        public List<ListaCompra> GetAbertas()
        {
            return GetByEstado("Aberta");
        }

        // Devolve uma lista completa com todos os seus itens, artigos e tipos carregados
        // Usado no FormModoCompra para ter todos os dados disponíveis sem lazy loading
        public ListaCompra GetById(int id)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.ListasCompras
                        .Include(l => l.UtilizadorCriou)
                        .Include(l => l.UtilizadorAlterou)
                        .Include(l => l.Itens.Select(i => i.Artigo.Tipo))
                        .FirstOrDefault(l => l.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter lista de compras: " + ex.Message);
            }
        }

        // Devolve apenas os itens previstos de uma lista, com o Artigo e o Tipo carregados
        // Usado no FormCriacaoEdicaoCompra para preencher a grelha de itens
        // NOTA: usa Include com strings em vez de lambdas porque o .OfType<>()
        //       não é compatível com Include por lambda no Entity Framework 6
        public List<ItemPrevisto> GetItensPrevistos(int listaId)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.ItensCompra
                        .OfType<ItemPrevisto>()
                        .Where(i => i.ListaCompraId == listaId)
                        .Include("Artigo")       // carrega o artigo associado ao item
                        .Include("Artigo.Tipo")  // carrega o tipo do artigo
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter itens previstos: " + ex.Message);
            }
        }

        // ── ESCRITA — LISTAS ──────────────────────────────────────────────────

        // Cria uma nova lista de compras com estado "Aberta"
        // Regista o utilizador criador e a data de criação
        public bool Add(string nome, int utilizadorCriouId, out string mensagem)
        {
            mensagem = "";

            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome da compra não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Cria a nova lista com dados de auditoria
                    ListaCompra novaLista = new ListaCompra();
                    novaLista.Nome = nome.Trim();
                    novaLista.DataCriacao = DateTime.Now;
                    novaLista.DataAlteracao = DateTime.Now;
                    novaLista.Estado = "Aberta";           // começa sempre aberta
                    novaLista.UtilizadorCriouId = utilizadorCriouId;
                    novaLista.UtilizadorAlterouId = utilizadorCriouId;

                    db.ListasCompras.Add(novaLista);
                    db.SaveChanges();

                    mensagem = "Lista de compras criada com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao criar lista: " + ex.Message;
                return false;
            }
        }

        // Atualiza o nome de uma lista
        // Regra: só é permitido alterar listas com estado "Aberta"
        public bool Update(int id, string nome, int utilizadorAlterouId, out string mensagem)
        {
            mensagem = "";

            // Validação: o nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome da compra não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    ListaCompra lista = db.ListasCompras.Find(id);

                    if (lista == null)
                    {
                        mensagem = "Lista não encontrada.";
                        return false;
                    }

                    // Regra de negócio: listas fechadas não podem ser editadas
                    if (lista.Estado == "Fechada")
                    {
                        mensagem = "Não é possível alterar uma compra fechada.";
                        return false;
                    }

                    // Atualiza o nome e regista a auditoria de alteração
                    lista.Nome = nome.Trim();
                    lista.DataAlteracao = DateTime.Now;
                    lista.UtilizadorAlterouId = utilizadorAlterouId;

                    db.SaveChanges();

                    mensagem = "Lista atualizada com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar lista: " + ex.Message;
                return false;
            }
        }

        // Elimina uma lista e todos os seus itens
        // O Include("Itens") garante que o EF elimina os itens em cascata
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Carrega a lista com os seus itens para garantir eliminação em cascata
                    ListaCompra lista = db.ListasCompras
                        .Include(l => l.Itens)
                        .FirstOrDefault(l => l.Id == id);

                    if (lista == null)
                    {
                        mensagem = "Lista não encontrada.";
                        return false;
                    }

                    db.ListasCompras.Remove(lista);
                    db.SaveChanges();

                    mensagem = "Lista eliminada com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao eliminar lista: " + ex.Message;
                return false;
            }
        }

        // ── ESCRITA — ITENS ───────────────────────────────────────────────────

        // Adiciona um item previsto a uma lista aberta
        // Regras:
        //   - A lista tem de estar "Aberta"
        //   - A quantidade tem de ser maior que zero
        //   - O mesmo artigo não pode ser adicionado duas vezes à mesma lista
        public bool AddItemPrevisto(int listaId, int artigoId, int quantidade, out string mensagem)
        {
            mensagem = "";

            // Validação: quantidade tem de ser positiva
            if (quantidade <= 0)
            {
                mensagem = "A quantidade tem de ser maior que zero.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    ListaCompra lista = db.ListasCompras.Find(listaId);

                    if (lista == null)
                    {
                        mensagem = "Lista não encontrada.";
                        return false;
                    }

                    // Regra de negócio: não pode adicionar itens a uma lista fechada
                    if (lista.Estado == "Fechada")
                    {
                        mensagem = "A compra está fechada.";
                        return false;
                    }

                    // Regra de negócio: não pode repetir o mesmo artigo na mesma lista
                    bool artigoJaAdicionado = db.ItensCompra
                        .OfType<ItemPrevisto>()
                        .Any(i => i.ListaCompraId == listaId && i.ArtigoId == artigoId);

                    if (artigoJaAdicionado)
                    {
                        mensagem = "Este artigo já foi adicionado à lista.";
                        return false;
                    }

                    // Cria o novo item previsto com quantidade adquirida e preço a zero
                    // (serão preenchidos durante a execução da compra no FormModoCompra)
                    ItemPrevisto novoItem = new ItemPrevisto();
                    novoItem.ListaCompraId = listaId;
                    novoItem.ArtigoId = artigoId;
                    novoItem.QuantidadePrevista = quantidade;
                    novoItem.QuantidadeAdquirida = 0;
                    novoItem.PrecoUnitario = 0;
                    novoItem.UtilizadorId = lista.UtilizadorCriouId;

                    db.ItensCompra.Add(novoItem);
                    db.SaveChanges();

                    mensagem = "Item adicionado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao adicionar item: " + ex.Message;
                return false;
            }
        }

        // Remove um item previsto da lista pelo Id do item
        public bool RemoveItemPrevisto(int itemId, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Vai à BD buscar o item pelo Id, filtrando apenas ItemPrevisto
                    ItemPrevisto item = db.ItensCompra
                        .OfType<ItemPrevisto>()
                        .FirstOrDefault(i => i.Id == itemId);

                    if (item == null)
                    {
                        mensagem = "Item não encontrado.";
                        return false;
                    }

                    db.ItensCompra.Remove(item);
                    db.SaveChanges();

                    mensagem = "Item removido!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao remover item: " + ex.Message;
                return false;
            }
        }
    }
}