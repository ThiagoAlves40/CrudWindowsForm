using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Aula28_09
{
    public partial class frmCadAtendente : Form
    {
        public frmCadAtendente()
        {
            InitializeComponent();
        }

        private void limparTextBoxes(Control.ControlCollection controles)
        {
            //Faz um laço para todos os controles passados no parâmetro
            foreach (Control ctrl in controles)
            {
                //Se o contorle for um TextBox...
                if (ctrl is TextBox)
                {
                    ((TextBox)(ctrl)).Text = String.Empty;
                }
            }
        }

        private void desabilitaCampos()
        {
            txtNome.Enabled = false;
            txtLogin.Enabled = false;
            txtSenha.Enabled = false;
            btnAlterar.Enabled = false;
            btnCancelar.Enabled = false;
            btnRemover.Enabled = false;
            btnSalvar.Enabled = false;
        }

        private void habilitarCampos()
        {
            txtNome.Enabled = true;
            txtLogin.Enabled = true;
            txtSenha.Enabled = true;
            txtNome.Focus();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
        }

        private void btnTestaConexao_Click(object sender, EventArgs e)
        {
            SqlConnection conn = conexao.obterConexao();

            // a conexão foi efetuada com sucesso?
            if (conn == null)
            {
                MessageBox.Show("Não foi possível obter a conexão. Veja o log de erros.");
            }

            else
            {
                MessageBox.Show("A conexão foi obtida com sucesso.");
            }

            // não precisamos mais da conexão? vamos fechá-la
            conexao.fecharConexao();
        }

        private void frmCadAtendente_Load(object sender, EventArgs e)
        {
            desabilitaCampos();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = conexao.obterConexao();
            SqlCommand objComandoSql = new SqlCommand();

            objComandoSql.Connection = conn;

            btnNovo.Enabled = false;

            if (txtNome.Text == "")
            {
                MessageBox.Show("Campo nome é obrigatório");
                txtNome.Focus();
            }

            else if (txtLogin.Text == "")
            {
                MessageBox.Show("Campo login é obrigatório");
                txtLogin.Focus();
            }

            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Campo senha é obrigatório");
                txtSenha.Focus();
            }

            else
            {
                try
                {
                    string login = txtLogin.Text;
                    string nome = txtNome.Text;
                    string senha = txtSenha.Text;

                    string strSql = $"insert into tbl_atendente (ds_login,ds_senha,nm_atendente) values ('{login}','{senha}','{nome}')";

                    objComandoSql = new SqlCommand(strSql, conn);
                    //conn.Open();
                    objComandoSql.ExecuteNonQuery();
                }

                catch (Exception erro)
                {
                    MessageBox.Show("" + erro);
                    conn.Close();
                }

                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparTextBoxes(this.Controls);
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = conexao.obterConexao();
            SqlCommand objComandoSql = new SqlCommand();

            objComandoSql.Connection = conn;
            if (txtBuscar.Text != "")
            {
                try
                {
                    objComandoSql.CommandText = "Select * from tbl_atendente where nm_atendente like ('%" + txtBuscar.Text + "%')";
                    objComandoSql.Connection = conn;

                    //recebe os dados de uma tabela apos a execuçã de uma select
                    SqlDataAdapter da = new SqlDataAdapter();

                    DataTable dt = new DataTable();

                    //recebe os dados da instrução select
                    da.SelectCommand = objComandoSql;
                    da.Fill(dt); //preenche o data table

                    dgvfunc.DataSource = dt;
                }

                catch (Exception erro)
                {
                    MessageBox.Show("\n" + erro);
                    conn.Close();
                }
            }

            else
            {
                dgvfunc.DataSource = null;
                conn.Close();
            }
        }

        private void carregarAtendente()
        {
            lblCod.Text = dgvfunc.SelectedRows[0].Cells[0].Value.ToString();
            txtLogin.Text = dgvfunc.SelectedRows[0].Cells[1].Value.ToString();
            txtSenha.Text = dgvfunc.SelectedRows[0].Cells[2].Value.ToString();
            txtNome.Text = dgvfunc.SelectedRows[0].Cells[3].Value.ToString();
            habilitarCampos();
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;
            btnAlterar.Enabled = true;
            btnRemover.Enabled = true;
            btnCancelar.Enabled = true;
            btnLimpar.Enabled = false;
        }

        private void dgvfunc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            carregarAtendente();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = conexao.obterConexao();
            SqlCommand objComandoSql = new SqlCommand();

            if (txtNome.Text == "")
            {
                MessageBox.Show("Campo nome é obrigatório");
                txtNome.Focus();
            }

            else if (txtLogin.Text == "")
            {
                MessageBox.Show("Login vazio");
                txtLogin.Focus();
            }

            else if (txtSenha.Text == "" && txtSenha.Text.Length < 8)
            {
                MessageBox.Show("Senha vazio / menos de 8 caractres");
                txtSenha.Focus();
            }

            else
            {
                try
                {
                    string login = txtLogin.Text;
                    string senha = txtSenha.Text;
                    string nome = txtNome.Text;
                    int cd = Convert.ToInt32(lblCod.Text);

                    string strSql = "update tbl_atendente set ds_login = @login,ds_senha=@senha,nm_atendente=@nome where cd_atendente=@cd";

                    objComandoSql.CommandText = strSql;
                    objComandoSql.Connection = conn;

                    objComandoSql.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    objComandoSql.Parameters.Add("@senha", SqlDbType.Char).Value = senha;
                    objComandoSql.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome;
                    objComandoSql.Parameters.Add("@cd", SqlDbType.Int).Value = cd;

                    // conn.Open();
                    objComandoSql.ExecuteNonQuery();
                    objComandoSql.Parameters.Clear();

                    MessageBox.Show("Dados alterados com sucesso");
                    //limparCampos();
                    dgvfunc.Refresh();
                }

                catch (Exception erro)
                {
                    MessageBox.Show("Algo deu Ruim\n" + erro.Message);
                    conn.Close();
                }

                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            SqlConnection conn = conexao.obterConexao();
            SqlCommand objComandoSql = new SqlCommand();

            try
            {
                if (DialogResult.OK == MessageBox.Show("Deseja remover funcionario?", "Remover",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    int cd = Convert.ToInt32(lblCod.Text);
                    string strSql = $"delete from tbl_atendente where cd_atendente = {cd};";

                    objComandoSql.CommandText = strSql;
                    objComandoSql.Connection = conn;
                    objComandoSql.ExecuteNonQuery();
                }

                else
                {
                    MessageBox.Show("Funcionario mantido");
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                conn.Close();
            }
        }
    }
}
