using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class ListaCompraController
    {
        // Devolve todas as listas de compras ordenadas da mais recente
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

        // Devolve uma lista completa com todos os seus itens e artigos carregados
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

        // Cria uma nova lista de compras com estado "Aberta"
        public bool Add(string nome, int utilizadorCriouId, out string mensagem)
        {
            mensagem = "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome da compra não pode estar vazio.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    ListaCompra novaLista = new ListaCompra();
                    novaLista.Nome = nome.Trim();
                    novaLista.DataCriacao = DateTime.Now;
                    novaLista.DataAlteracao = DateTime.Now;
                    novaLista.Estado = "Aberta";
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

        // Atualiza o nome de uma lista. Só permitido se a lista estiver "Aberta"
        public bool Update(int id, string nome, int utilizadorAlterouId, out string mensagem)
        {
            mensagem = "";

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

                    if (lista.Estado == "Fechada")
                    {
                        mensagem = "Não é possível alterar uma compra fechada.";
                        return false;
                    }

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
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
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

        // Devolve apenas os itens previstos de uma lista
        public List<ItemPrevisto> GetItensPrevistos(int listaId)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.ItensCompra
                        .OfType<ItemPrevisto>()
                        .Where(i => i.ListaCompraId == listaId)
                        .Include(i => i.Artigo.Tipo)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter itens previstos: " + ex.Message);
            }
        }

        // Adiciona um item previsto a uma lista aberta
        public bool AddItemPrevisto(int listaId, int artigoId, int quantidade, out string mensagem)
        {
            mensagem = "";

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

                    if (lista.Estado == "Fechada")
                    {
                        mensagem = "A compra está fechada.";
                        return false;
                    }

                    bool artigoJaAdicionado = db.ItensCompra
                        .OfType<ItemPrevisto>()
                        .Any(i => i.ListaCompraId == listaId && i.ArtigoId == artigoId);

                    if (artigoJaAdicionado)
                    {
                        mensagem = "Este artigo já foi adicionado à lista.";
                        return false;
                    }

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

        // Remove um item previsto da lista
        public bool RemoveItemPrevisto(int itemId, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
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
