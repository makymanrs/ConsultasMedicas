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
        public void eliminarTratamiento(DataGridView tablaTratamiento)
        {
            // Verificar si hay una fila seleccionada
            if (tablaTratamiento.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un tratamiento para eliminar.");
                return;
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Obtener el id_tratamiento de la fila seleccionada
                int idTratamiento = Convert.ToInt32(tablaTratamiento.SelectedRows[0].Cells["ID Tratamiento"].Value);

                // Eliminar el tratamiento de la tabla Tratamientos
                string queryTratamiento = "DELETE FROM Tratamientos WHERE id_tratamiento = @idTratamiento";
                MySqlCommand commandTratamiento = new MySqlCommand(queryTratamiento, conexion);
                commandTratamiento.Parameters.AddWithValue("@idTratamiento", idTratamiento);

                int rowsAffected = commandTratamiento.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se eliminó el registro del tratamiento.");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún tratamiento con ese ID.");
                }

                // Opcional: Actualizar el DataGridView después de la eliminación
                // Puedes implementar una función para volver a cargar los datos en tablaTratamiento
               // Asegúrate de tener este método en tu clase Ctratamiento
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el registro del tratamiento. Error: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void modificarTratamiento(int idTratamiento, TextBox medicamento, TextBox dosis, TextBox duracion, RichTextBox observaciones)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();
                string observacionesConViñetas = FormatearTextoConViñetas(observaciones);
                string query = "UPDATE Tratamientos SET medicamento=@medicamento, dosis=@dosis, duracion=@duracion, observaciones=@observaciones WHERE id_tratamiento=@idTratamiento";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idTratamiento", idTratamiento);
                command.Parameters.AddWithValue("@medicamento", medicamento.Text);
                command.Parameters.AddWithValue("@dosis", dosis.Text);
                command.Parameters.AddWithValue("@duracion", duracion.Text);
                command.Parameters.AddWithValue("@observaciones", observaciones.Text);

                command.ExecuteNonQuery();
                MessageBox.Show("Se modificaron los registros del tratamiento correctamente");

                // Opcional: actualizar la tabla para mostrar los cambios
                // Puedes volver a cargar los datos en tablaTratamiento
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo modificar el registro del tratamiento: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        private string FormatearTextoConViñetas(RichTextBox richTextBox)
        {
            // Convierte el contenido del RichTextBox a texto con viñetas
            StringBuilder sb = new StringBuilder();
            foreach (string line in richTextBox.Lines)
            {
                sb.AppendLine("• " + line);  // Usar "• " para agregar viñetas
            }
            return sb.ToString().Trim();
        }
    }
}
