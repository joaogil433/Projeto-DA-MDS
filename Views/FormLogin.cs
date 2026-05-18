using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projeto_DA_MDS;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            tabControl1.SelectedTab = tabPage2;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (var db = new IshoppingContext())
            {
                var utilizadorRegisto = db.Utilizadores.FirstOrDefault(u => u.Username == tbUsername.Text && u.Password == tbPassword.Text);

                if (utilizadorRegisto != null)
                {
                    MessageBox.Show("Sessão inicada com sucesso!");
                    SessaoUtilizador.Atual = utilizadorRegisto;
                    Form1 form = new Form1();
                    form.ShowDialog();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dados Inválidos!");
                    tbUsername.Clear();
                    tbPassword.Clear();
                    tbUsername.Focus();
                }
            }
        }

        private void btnRegisto_Click(object sender, EventArgs e)
        {
            using (var db = new IshoppingContext())
            {
                if(db.Utilizadores.Any(u => u.Username == tbUsername.Text))
                {
                    MessageBox.Show("Esse nome de utilizador já se encontra em uso!");
                    return;
                }

                var novoUser = new Utilizador
                {
                    Nome = tbNomeReg.Text,
                    Username = tbUsernameReg.Text,
                    Password = tbPasswordReg.Text
                };

                db.Utilizadores.Add(novoUser);
                db.SaveChanges();

                MessageBox.Show("Utilizador registado com sucesso!");

                tbUsernameReg.Clear();
                tbPasswordReg.Clear();
                tabControl1.SelectedTab = tabPage2;
            }
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
