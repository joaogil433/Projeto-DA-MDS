using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projeto_DA_MDS.Helpers;

namespace Projeto_DA_MDS.Controllers
{
    internal class LoginController
    {
        public static Utilizador Login(string username, string password)
        {
            string passwordHash = HashHelper.HashPassword(password);

            using (var db = new IshoppingContext())
            {
                return db.Utilizadores
                    .FirstOrDefault(u => u.Username == username
                                      && u.Password == passwordHash);
            }
        }
    }
}
