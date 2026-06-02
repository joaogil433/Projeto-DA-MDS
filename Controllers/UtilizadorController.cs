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
        public List<Utilizador> GetAll()
        {
            using (var db = new IshoppingContext())
            {
                return db.Utilizadores.OrderBy(u => u.Nome).ToList();
            }
        }

        public Utilizador GetById(int id)
        {
            using (var db = new IshoppingContext())
            {
                return db.Utilizadores.Find(id);
            }
        }

        public bool Add(string nome, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)) return false;

            using (var db = new IshoppingContext())
            {
                bool existe = db.Utilizadores.Any(u =>
                    u.Username.ToLower() == username.Trim().ToLower());

                if (existe) return false;

                db.Utilizadores.Add(new Utilizador
                {
                    Nome = nome.Trim(),
                    Username = username.Trim(),
                    Password = HashHelper.HashPassword(password),
                    CriadoPorId = Sessao.UtilizadorAtual?.Id
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

        public bool Update(int id, string nome, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(username)) return false;

            using (var db = new IshoppingContext())
            {
                Utilizador utilizador = db.Utilizadores.Find(id);
                if (utilizador == null) return false;

                bool duplicado = db.Utilizadores.Any(u =>
                    u.Username.ToLower() == username.Trim().ToLower() && u.Id != id);

                if (duplicado) return false;

                utilizador.Nome = nome.Trim();
                utilizador.Username = username.Trim();
                utilizador.AlteradoPorId = Sessao.UtilizadorAtual?.Id;

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

        public bool Delete(int id)
        {
            using (var db = new IshoppingContext())
            {
                Utilizador utilizador = db.Utilizadores.Find(id);
                if (utilizador == null) return false;

                if (Sessao.UtilizadorAtual != null && Sessao.UtilizadorAtual.Id == id)
                    return false;

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
