// OrcamentoController.cs
// Responsabilidade: lógica de negócio para os orçamentos mensais (US2)
// Regras principais:
//   - Só pode existir um orçamento por mês/ano
//   - O valor máximo tem de ser maior que zero
//   - Regista sempre quem criou e quem alterou

using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class OrcamentoController
    {
        // ── LEITURA ───────────────────────────────────────────────────────────

        // Devolve todos os orçamentos ordenados do mais recente para o mais antigo
        // Carrega também os utilizadores que criaram e alteraram (para mostrar auditoria na View)
        public List<Orcamento> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Orcamentos
                        .Include(o => o.UtilizadorCriou)    // carrega o utilizador que criou
                        .Include(o => o.UtilizadorAlterou)  // carrega o utilizador que alterou
                        .OrderByDescending(o => o.Ano)
                        .ThenByDescending(o => o.Mes)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter orçamentos: " + ex.Message);
            }
        }

        // Devolve um orçamento pelo Id — usado para preencher o formulário de edição
        public Orcamento GetById(int id)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Orcamentos
                        .Include(o => o.UtilizadorCriou)
                        .Include(o => o.UtilizadorAlterou)
                        .FirstOrDefault(o => o.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter orçamento: " + ex.Message);
            }
        }

        // ── ESCRITA ───────────────────────────────────────────────────────────

        // Cria um novo orçamento mensal
        // Regra de negócio: só pode existir um orçamento por mês/ano
        // Devolve true se criado com sucesso; false com mensagem de erro em caso de falha
        public bool Add(decimal valor, int mes, int ano, int utilizadorCriadorId, out string mensagem)
        {
            mensagem = "";

            // Validação do valor — tem de ser positivo
            if (valor <= 0)
            {
                mensagem = "O valor do orçamento tem de ser maior que zero.";
                return false;
            }

            // Validação do mês — entre 1 e 12
            if (mes < 1 || mes > 12)
            {
                mensagem = "Mês inválido.";
                return false;
            }

            // Validação do ano — intervalo razoável
            if (ano < 2000 || ano > 2100)
            {
                mensagem = "Ano inválido.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Verifica se já existe orçamento para este mês/ano
                    bool existe = db.Orcamentos.Any(o => o.Mes == mes && o.Ano == ano);

                    if (existe)
                    {
                        mensagem = "Já existe um orçamento para " + mes.ToString("D2") + "/" + ano + ".";
                        return false;
                    }

                    // Cria o novo orçamento com dados de auditoria
                    Orcamento novoOrcamento = new Orcamento();
                    novoOrcamento.ValorMaximo = valor;
                    novoOrcamento.Mes = mes;
                    novoOrcamento.Ano = ano;
                    novoOrcamento.DataCriacao = DateTime.Now;
                    novoOrcamento.DataAlteracao = DateTime.Now;
                    novoOrcamento.UtilizadorCriouId = utilizadorCriadorId;
                    novoOrcamento.UtilizadorAlterouId = utilizadorCriadorId;

                    db.Orcamentos.Add(novoOrcamento);
                    db.SaveChanges();

                    mensagem = "Orçamento criado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao criar orçamento: " + ex.Message;
                return false;
            }
        }

        // Atualiza um orçamento existente
        // Verifica duplicado de mês/ano excluindo o próprio orçamento que está a ser editado
        // Regista quem alterou e quando
        public bool Update(int id, decimal valor, int mes, int ano, int utilizadorAlterouId, out string mensagem)
        {
            mensagem = "";

            // Validações — iguais às do Add
            if (valor <= 0)
            {
                mensagem = "O valor do orçamento tem de ser maior que zero.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                mensagem = "Mês inválido.";
                return false;
            }

            if (ano < 2000 || ano > 2100)
            {
                mensagem = "Ano inválido.";
                return false;
            }

            try
            {
                using (var db = new IshoppingContext())
                {
                    // Vai à BD buscar o orçamento pelo Id
                    Orcamento orcamento = db.Orcamentos.Find(id);

                    if (orcamento == null)
                    {
                        mensagem = "Orçamento não encontrado.";
                        return false;
                    }

                    // Verifica conflito de mês/ano mas exclui o próprio registo da verificação
                    bool conflito = db.Orcamentos.Any(o =>
                        o.Mes == mes && o.Ano == ano && o.Id != id);

                    if (conflito)
                    {
                        mensagem = "Já existe um orçamento para " + mes.ToString("D2") + "/" + ano + ".";
                        return false;
                    }

                    // Atualiza os campos e regista a auditoria de alteração
                    orcamento.ValorMaximo = valor;
                    orcamento.Mes = mes;
                    orcamento.Ano = ano;
                    orcamento.DataAlteracao = DateTime.Now;
                    orcamento.UtilizadorAlterouId = utilizadorAlterouId;

                    db.SaveChanges();

                    mensagem = "Orçamento atualizado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar orçamento: " + ex.Message;
                return false;
            }
        }

        // Elimina um orçamento pelo Id
        // Devolve true se eliminado; false se não encontrado ou erro
        public bool Delete(int id, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new IshoppingContext())
                {
                    Orcamento orcamento = db.Orcamentos.Find(id);

                    if (orcamento == null)
                    {
                        mensagem = "Orçamento não encontrado.";
                        return false;
                    }

                    db.Orcamentos.Remove(orcamento);
                    db.SaveChanges();

                    mensagem = "Orçamento eliminado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao eliminar orçamento: " + ex.Message;
                return false;
            }
        }

        // ── CÁLCULO ───────────────────────────────────────────────────────────

        // Calcula quanto dinheiro ainda sobra no orçamento de um dado mês/ano
        // Soma todos os gastos das compras criadas nesse mês e subtrai ao valor máximo
        // Devolve 0 se não existir orçamento definido para o mês/ano pedido
        public decimal GetOrcamentoDisponivel(int mes, int ano)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    // Tenta encontrar o orçamento para o mês/ano pedido
                    Orcamento orcamento = db.Orcamentos
                        .FirstOrDefault(o => o.Mes == mes && o.Ano == ano);

                    // Se não há orçamento definido, devolve 0
                    if (orcamento == null)
                        return 0;

                    // Soma o total gasto em todas as compras do mês
                    // (QuantidadeAdquirida × PrecoUnitario por cada item)
                    // Usa decimal? para evitar NullReferenceException quando não há itens
                    decimal totalGasto = db.ListasCompras
                        .Where(l => l.DataCriacao.Month == mes && l.DataCriacao.Year == ano)
                        .SelectMany(l => l.Itens)
                        .Sum(i => (decimal?)(i.QuantidadeAdquirida * i.PrecoUnitario)) ?? 0m;

                    return orcamento.ValorMaximo - totalGasto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular orçamento disponível: " + ex.Message);
            }
        }
    }
}