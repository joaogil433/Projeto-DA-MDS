using Projeto_DA_MDS.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DA_MDS.Controllers
{
    public class OrcamentoController
    {
        // Devolve todos os orçamentos ordenados do mais recente para o mais antigo
        public List<Orcamento> GetAll()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    return db.Orcamentos
                        .Include(o => o.UtilizadorCriou)
                        .Include(o => o.UtilizadorAlterou)
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

        // Cria um novo orçamento mensal. Regra: só pode existir um por mês/ano
        public bool Add(decimal valor, int mes, int ano, int utilizadorCriadorId, out string mensagem)
        {
            mensagem = "";

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
                    bool existe = db.Orcamentos.Any(o => o.Mes == mes && o.Ano == ano);

                    if (existe)
                    {
                        mensagem = "Já existe um orçamento para " + mes.ToString("D2") + "/" + ano + ".";
                        return false;
                    }

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

        // Atualiza um orçamento existente, registando quem alterou e quando
        public bool Update(int id, decimal valor, int mes, int ano, int utilizadorAlterouId, out string mensagem)
        {
            mensagem = "";

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
                    Orcamento orcamento = db.Orcamentos.Find(id);

                    if (orcamento == null)
                    {
                        mensagem = "Orçamento não encontrado.";
                        return false;
                    }

                    // Verifica conflito mas exclui o próprio orçamento
                    bool conflito = db.Orcamentos.Any(o =>
                        o.Mes == mes && o.Ano == ano && o.Id != id);

                    if (conflito)
                    {
                        mensagem = "Já existe um orçamento para " + mes.ToString("D2") + "/" + ano + ".";
                        return false;
                    }

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

        // Elimina um orçamento
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

        // Calcula quanto dinheiro ainda sobra no orçamento de um dado mês/ano
        public decimal GetOrcamentoDisponivel(int mes, int ano)
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    Orcamento orcamento = db.Orcamentos
                        .FirstOrDefault(o => o.Mes == mes && o.Ano == ano);

                    if (orcamento == null)
                    {
                        return 0;
                    }

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
