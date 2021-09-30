using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Aula28_09
{
    public class conexao
    {
        // vamos nos conectar ao SQL Server Express e à base de dados
        // locadora usando Windows Authentication
        private static string connString = @"server = ARADSK273\SQLEXPRESS;
        Database = db_livraria;integrated security = true;User Id=CASTBSB\thiago.alves";

        // representa a conexão com o banco
        private static SqlConnection conn = null;

        // método que permite obter a conexão
        public static SqlConnection obterConexao()
        {
            // vamos criar a conexão
            SqlConnection conn = new SqlConnection(connString);

            // a conexão foi feita com sucesso?
            try
            {
                // abre a conexão e a devolve ao chamador do método
                conn.Open();
            }

            catch (SqlException sqle)
            {
                conn = null;
                // ops! o que aconteceu?
                // uma boa idéia aqui é gravar a exceção em um arquivo de log
            }

            return conn;
        }

        public static void fecharConexao()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}
