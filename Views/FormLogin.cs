using Projeto_DA_MDS;
using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projeto_DA_MDS.Helpers;

namespace Projeto_DA_MDS.Views
{
    public partial class FormLogin : Form
    {
        private LoginController loginCtrl;
        public FormLogin()
        {
            InitializeComponent();
            loginCtrl = new LoginController();
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

            Utilizador utilizador = loginCtrl.Login(tbUsername.Text, tbPassword.Text);

            if (utilizador != null)
            {
                Sessao.UtilizadorAtual = utilizador;
                FormPrincipal form = new FormPrincipal();
                form.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username ou password incorretos.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Clear();
                tbPassword.Clear();
                tbUsername.Focus();
            }


        }

        private void btnRegisto_Click(object sender, EventArgs e)
        {
            UtilizadorController utilizadorCtrl = new UtilizadorController();

            bool sucesso = utilizadorCtrl.Add(tbNomeReg.Text, tbUsernameReg.Text, tbPasswordReg.Text);

            if (sucesso)
            {
                MessageBox.Show("Utilizador registado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbNomeReg.Clear();
                tbUsernameReg.Clear();
                tbPasswordReg.Clear();
                tabControl1.SelectedTab = tabPage2;
            }
            else
            {
                MessageBox.Show("Erro ao registar. Verifique se todos os campos estão preenchidos " +
                    "e se o username não está em uso.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tbNomeReg_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
