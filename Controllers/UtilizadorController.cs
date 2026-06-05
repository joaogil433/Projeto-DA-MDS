// UtilizadorController.cs
// Responsabilidade: lógica de negócio para a gestão de utilizadores (US8)
// Regras principais:
//   - Username tem de ser único
//   - A password é sempre guardada em hash (nunca em texto simples)
//   - Não é possível eliminar o utilizador com sessão iniciada
//   - Não é possível eliminar utilizadores com dados associados (compras ou orçamentos)

using Projeto_DA_MDS.Helpers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Controllers
{
    internal class UtilizadorController
    {
        // ── LEITURA ───────────────────────────────────────────────────────────

        // Devolve todos os utilizadores ordenados por nome
        public List<Utilizador> GetAll()
        {
            using (var db = new IshoppingContext())
            {
                return db.Utilizadores.OrderBy(u => u.Nome).ToList();
            }
        }

        // Devolve um utilizador pelo Id — usado para preencher o formulário de edição
        public Utilizador GetById(int id)
        {
            using (var db = new IshoppingContext())
            {
                return db.Utilizadores.Find(id);
            }
        }

        // ── ESCRITA ───────────────────────────────────────────────────────────

        // Cria um novo utilizador
        // A password é convertida para hash antes de ser guardada — nunca é guardada em texto simples
        // Devolve false se os campos estiverem vazios ou se o username já existir
        public bool Add(string nome, string username, string password)
        {
            // Validação: todos os campos são obrigatórios
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)) return false;

            using (var db = new IshoppingContext())
            {
                // Verifica se já existe um utilizador com o mesmo username (case-insensitive)
                bool existe = db.Utilizadores.Any(u =>
                    u.Username.ToLower() == username.Trim().ToLower());

                if (existe) return false;

                // Cria o utilizador com a password em hash e regista quem criou
                db.Utilizadores.Add(new Utilizador
                {
                    Nome = nome.Trim(),
                    Username = username.Trim(),
                    Password = HashHelper.HashPassword(password),  // hash SHA256 da password
                    CriadoPorId = Sessao.UtilizadorAtual?.Id          // registo de auditoria
                });

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

        // Atualiza os dados de um utilizador existente
        // Se a password vier vazia, mantém a password atual (não a altera)
        // Se vier preenchida, converte para hash e atualiza
        public bool Update(int id, string nome, string username, string password)
        {
            // Validação: nome e username são obrigatórios; password é opcional na edição
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(username)) return false;

            using (var db = new IshoppingContext())
            {
                // Vai à BD buscar o utilizador pelo Id
                Utilizador utilizador = db.Utilizadores.Find(id);
                if (utilizador == null) return false;

                // Verifica duplicado de username excluindo o próprio utilizador que está a ser editado
                bool duplicado = db.Utilizadores.Any(u =>
                    u.Username.ToLower() == username.Trim().ToLower() && u.Id != id);

                if (duplicado) return false;

                // Atualiza os campos e regista quem alterou
                utilizador.Nome = nome.Trim();
                utilizador.Username = username.Trim();
                utilizador.AlteradoPorId = Sessao.UtilizadorAtual?.Id;

                // Só atualiza a password se foi fornecida uma nova
                if (!string.IsNullOrWhiteSpace(password))
                    utilizador.Password = HashHelper.HashPassword(password);

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

        // Elimina um utilizador pelo Id
        // Proteções:
        //   1. Não pode eliminar o utilizador com sessão iniciada
        //   2. Não pode eliminar utilizadores com compras ou orçamentos associados
        public bool Delete(int id)
        {
            using (var db = new IshoppingContext())
            {
                // Vai à BD buscar o utilizador pelo Id
                Utilizador utilizador = db.Utilizadores.Find(id);
                if (utilizador == null) return false;

                // Proteção 1: não pode eliminar a si próprio
                if (Sessao.UtilizadorAtual != null && Sessao.UtilizadorAtual.Id == id)
                    return false;

                // Proteção 2: não pode eliminar se tiver dados associados
                // (compras criadas ou orçamentos criados por este utilizador)
                bool temDados = db.ListasCompras.Any(l => l.UtilizadorCriouId == id) ||
                                db.Orcamentos.Any(o => o.UtilizadorCriouId == id);

                if (temDados) return false;

                db.Utilizadores.Remove(utilizador);

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