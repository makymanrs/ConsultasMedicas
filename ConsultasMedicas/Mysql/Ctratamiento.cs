using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas.Mysql
{
    internal class Ctratamiento
    {
        public void BuscarEnfermedadPorCodigo(TextBox textBoxCodigoEnfermedad, TextBox textBoxNombreEnfermedad)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                string query = @"SELECT nombre
                         FROM Enfermedades
                         WHERE id_enfermedad = @CodigoEnfermedad";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    // Asegúrate de usar Trim() para eliminar espacios adicionales
                    cmd.Parameters.AddWithValue("@CodigoEnfermedad", textBoxCodigoEnfermedad.Text.Trim());

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxNombreEnfermedad.Text = reader["nombre"].ToString();
                        }
                        else
                        {
                            textBoxNombreEnfermedad.Text = "";
                            MessageBox.Show("No se encontró ninguna enfermedad con el código especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos de la enfermedad: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void InsertarTratamiento(string idEnfermedad, DataGridView dataGridTratamiento, MySqlConnection conexion)
        {
            try
            {
                string insertQuery = "INSERT INTO Tratamientos (id_enfermedad, medicamento, dosis, duracion, observaciones) VALUES (@IdEnfermedad, @Medicamento, @Dosis, @Duracion, @Observaciones)";

                using (MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conexion))
                {
                    foreach (DataGridViewRow row in dataGridTratamiento.Rows)
                    {
                        // Asegurarse de que la fila no sea la fila de nuevo
                        if (!row.IsNewRow)
                        {
                            if (row.Cells["medicamento"].Value != null && row.Cells["dosis"].Value != null && row.Cells["duracion"].Value != null && row.Cells["observaciones"].Value != null)
                            {
                                // Obtener los valores de las celdas
                                string medicamento = row.Cells["medicamento"].Value.ToString();
                                string dosis = row.Cells["dosis"].Value.ToString();
                                string duracion = row.Cells["duracion"].Value.ToString();
                                string observaciones = row.Cells["observaciones"].Value.ToString();

                                // Insertar detalle tratamiento
                                cmdInsert.Parameters.Clear();
                                cmdInsert.Parameters.AddWithValue("@IdEnfermedad", idEnfermedad);
                                cmdInsert.Parameters.AddWithValue("@Medicamento", medicamento);
                                cmdInsert.Parameters.AddWithValue("@Dosis", dosis);
                                cmdInsert.Parameters.AddWithValue("@Duracion", duracion);
                                cmdInsert.Parameters.AddWithValue("@Observaciones", observaciones);

                                cmdInsert.ExecuteNonQuery();
                                
                            }
                        }
                    }
                    MessageBox.Show("Se guardo los registros");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar los detalles del tratamiento: " + ex.Message);
            }
        }

    }
}
