using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ADONET___Projeto_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().testaMetodoRejectChanges();
            Console.ReadKey();
        }

        //Metodo para fazer a Conexão com o Banco de Dados.
        public void connection()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();
                System.Console.WriteLine("Conexão estabelecida com sucesso!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu Errado.");
            }
            finally
            {
                con.Close();
            }

        }

        //Metodo que vai retornar todos valores do Banco de Dados.
        public void testaExecuteReader()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();

                SqlCommand cm = new SqlCommand("select * from professor", con);
                SqlDataReader dataReader = cm.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader["Idprofessor"] + ") " + dataReader["matricula"] + " - " + dataReader["nome"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.\n" + e);
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo que vai Retornar a quantidade de Professores no Banco de Dados.
        public void testaExecuteScalar()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();

                SqlCommand cm = new SqlCommand("select count(idprofessor) from professor", con);
                int totalProfs = (int)cm.ExecuteScalar();
                Console.WriteLine("Números de professores cadastros: " + totalProfs);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.\n" + e);
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo que retorna a quantidade de linhas Inseridas, linhas Atualizadas e 
        // a quantidade de linhas deletadas.
        public void testaExecuteNonQuery()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();
                SqlCommand cmd = new SqlCommand(@"insert into professor(matricula, nome) values ('115349','Profe. Substituto')", con);
                int linhasAfetadas = cmd.ExecuteNonQuery();
                Console.WriteLine("Linhas Inseridas = " + linhasAfetadas);

                cmd.CommandText = "update professor set nome = 'Profe. Titular' where matricula = '115349'";
                linhasAfetadas = cmd.ExecuteNonQuery();
                Console.WriteLine("Linhas Atualizadas = " + linhasAfetadas);

                cmd.CommandText = "delete from professor where matricula = '115349'";
                linhasAfetadas = cmd.ExecuteNonQuery();
                Console.WriteLine("Linhas Deletadas = " + linhasAfetadas);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.\n" + e);
            }
            finally
            {
                con.Close();
            }
        }

        //Outro exemplo de metodo que retorna todos os valores do Banco de Dados.
        public void testaExecuteReaderIndex()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();

                SqlCommand cm = new SqlCommand("select * from professor", con);
                SqlDataReader dataReader = cm.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader[0] + ") " + dataReader[1] + " - " + dataReader[2]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.\n" + e);
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo que mostra multiplos result sets, mais de uma tabela.
        public void testaExecuteReaderMultiResultSets()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();
                SqlCommand cm = new SqlCommand("select * from professor; select * from curso;", con);
                SqlDataReader dataReader = cm.ExecuteReader();
                Console.WriteLine("Primeiro Result Set:\n");
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader[0] + ") " + dataReader[1] + " - " + dataReader[2]);
                }

                if (dataReader.NextResult())
                {
                    Console.WriteLine("Segundo Result Set:\n");
                    while (dataReader.Read())
                    {
                        Console.WriteLine(dataReader[0] + ") " + dataReader[1] + " - " + dataReader[2]);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.");
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo que testa o Data Set e o Data Table.
        public void testaSqlDataAdapter()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from professor", con);
                DataTable dt = new DataTable();

                da.Fill(dt);
                Console.WriteLine("Usando a Data Table");

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                    //Console.WriteLine(row[0] + " - " + row[1]);
                }
                Console.WriteLine("-----------------------------------");

                DataSet ds = new DataSet();
                da.Fill(ds, "Professor");
                Console.WriteLine("Usando o DataSet");

                foreach (DataRow row in ds.Tables["Professor"].Rows)
                {
                    Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                    //Console.WriteLine(row[1] + " - " + row[2]);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.");
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo que usa o Stored Procedure
        public void testaSqlDataAdapterStoresProcedure()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("spProfessores", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.");
            }
            finally
            {
                con.Close();
            }
        }

        //Exemplo de Data Table.
        public void testaSqlDataTableExemplo01()
        {
            DataTable dataTable = new DataTable("professor");

            DataColumn idprofessor = new DataColumn("idProfessor");
            idprofessor.DataType = typeof(int);
            idprofessor.Unique = true;
            idprofessor.AllowDBNull = true;
            idprofessor.Caption = "Id Professor";
            dataTable.Columns.Add(idprofessor);

            DataColumn matricula = new DataColumn("matricula");
            matricula.MaxLength = 100;
            matricula.AllowDBNull = false;
            dataTable.Columns.Add(matricula);

            DataColumn nome = new DataColumn("nome");
            nome.MaxLength = 100;
            nome.AllowDBNull = false;
            dataTable.Columns.Add(nome);

            DataRow row1 = dataTable.NewRow();
            row1["idprofessor"] = 1001;
            row1["matricula"] = "908760";
            row1["nome"] = "Marlon Samuel";
            dataTable.Rows.Add(row1);

            dataTable.Rows.Add(1002, "563262", "Carl johnson");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine(row["idprofessor"] + " - "
                    + row["matricula"] + " - " + row["nome"]);
            }
        }

        //Usando o Auto Incremento.
        public void testaSqlDataTableExemplo02()
        {
            DataTable dataTable = new DataTable("professor");

            DataColumn idprofessor = new DataColumn
            {
                ColumnName = "Idprofessor",
                DataType = System.Type.GetType("System.Int32"),
                AutoIncrement = true,
                AutoIncrementSeed = 1000,
                AutoIncrementStep = 10,
                Unique = true,
                AllowDBNull = false,
                Caption = "Id Professor",
            };

            dataTable.Columns.Add(idprofessor);

            DataColumn matricula = new DataColumn("matricula");
            matricula.MaxLength = 100;
            matricula.AllowDBNull = false;
            dataTable.Columns.Add(matricula);

            DataColumn nome = new DataColumn("nome");
            nome.MaxLength = 100;
            nome.AllowDBNull = false;
            dataTable.Columns.Add(nome);

            DataRow row1 = dataTable.NewRow();
            row1["matricula"] = "789236";
            row1["nome"] = "Cristiano Ronaldo";
            dataTable.Rows.Add(row1);

            dataTable.Rows.Add(null, "263725", "Lionel Messi");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine(row["idprofessor"] + " - "
                    + row["matricula"] + " - " + row["nome"]);
            }
        }

        //Usando os métodos Copy e Clone.
        public void testaMetodosDataTableCopyClone()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                SqlDataAdapter da = new SqlDataAdapter("select * from professor", con);
                DataTable originalDt = new DataTable();
                da.Fill(originalDt);

                foreach (DataRow row in originalDt.Rows)
                {
                    Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                }

                Console.WriteLine();
                Console.WriteLine("Copia da DataTable: copyDataTable");
                DataTable copyDataTable = originalDt.Copy();
                if (copyDataTable != null)
                {
                    foreach (DataRow row in copyDataTable.Rows)
                    {
                        Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Clone da DataTable: cloneDataTable");
                DataTable cloneDataTable = originalDt.Clone();
                if (cloneDataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in cloneDataTable.Rows)
                    {
                        Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                    }
                }
                else
                {
                    Console.WriteLine("cloneDataTable está vazia");
                    Console.WriteLine("Adicionando dados em cloneDataTable");
                    cloneDataTable.Rows.Add(1001, "112233", "Zé da Silva");
                    cloneDataTable.Rows.Add(10011, "332211", "Maria Xikinha");
                    foreach (DataRow row in cloneDataTable.Rows)
                    {
                        Console.WriteLine(row["matricula"] + " - " + row["nome"]);
                    }
                }

            } catch (Exception ex)
            {
                Console.WriteLine("Ops! Algo deu errado.");
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo Delete.
        public void testaMetodoDeleteDataRow()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                SqlDataAdapter da = new SqlDataAdapter("select * from professor", con);
                DataTable originalDT = new DataTable();
                da.Fill(originalDT);
                Console.WriteLine("Antes da Exclusão");

                foreach (DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
                Console.WriteLine();
                foreach (DataRow row in originalDT.Rows)
                {
                    if (Convert.ToInt32(row["idprofessor"]) == 3) {
                        row.Delete();
                        Console.WriteLine("Linha com professorid=3 foi Deletada");
                    }
                }
                originalDT.AcceptChanges();
                Console.WriteLine();
                Console.WriteLine("Depois da exclusão");
                foreach (DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado.");
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo Remove.
        public void testaMetodoRemoveDataRow()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                SqlDataAdapter da = new SqlDataAdapter("select * from professor",con);
                DataTable originalDT = new DataTable();
                da.Fill(originalDT);
                Console.WriteLine("Antes da exclusão");
                foreach(DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
                Console.WriteLine();
                foreach(DataRow row in originalDT.Select())
                {
                    if (Convert.ToInt32(row["idprofessor"]) == 3)
                    {
                        originalDT.Rows.Remove(row);
                        Console.WriteLine("Linha com idprofessor 3 Deletada");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Depois da exclusão");
                foreach (DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
            }catch (Exception e)
            {
                Console.WriteLine("OOPs, algo deu errado.\n" + e);
            }
            finally
            {
                con.Close();
            }
        }

        //Metodo Reject Changes.
        public void testaMetodoRejectChanges()
        {
            SqlConnection con = null;
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                con = new SqlConnection(ConString);
                SqlDataAdapter da = new SqlDataAdapter("select * from professor", con);
                DataTable originalDT = new DataTable();
                da.Fill(originalDT);
                Console.WriteLine("Antes da Exclusão");
                foreach (DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
                Console.WriteLine();
                foreach(DataRow row in originalDT.Rows)
                {
                    if (Convert.ToInt32(row["idprofessor"]) == 3)
                    {
                        row.Delete();
                        Console.WriteLine("Linha com idProfessor = 3, foi deletada");
                    }
                }
                //Rollback nos dados.
                originalDT.RejectChanges();
                Console.WriteLine();
                Console.WriteLine("Dando Rollback na exclusão");
                foreach(DataRow row in originalDT.Rows)
                {
                    Console.WriteLine(row["idprofessor"] + ", " + row["matricula"] + ", " + row["nome"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Ops! Algo deu errado." + e);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
