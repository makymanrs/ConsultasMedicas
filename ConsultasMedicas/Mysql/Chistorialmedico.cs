using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas.Mysql
{
    internal class Chistorialmedico
    {
        public void mostrarHistorialMedico(DataGridView tablaHistorial)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                string query = "SELECT " +
                               "h.id_historial as 'ID Historial', " +
                               "h.pac_nombre as 'Nombre del Paciente', " +
                               "h.fecha_consulta as 'Fecha de Consulta', " +
                               "h.enfermedad_nombre as 'Nombre de la Enfermedad', " +
                               "h.diagnostico as 'Diagnóstico', " +
                               "h.tratamiento as 'Tratamiento' " +
                               "FROM HistorialMedico h";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Asignar los datos al DataGridView
                tablaHistorial.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("No se encontraron los datos de la base de datos, error: " + ex.ToString());
            }
            finally
            {
                // Cerrar la conexión
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void BuscarPacientePorCodigo(TextBox textBoxCodigoPaciente, TextBox textBoxNombreCompleto)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                string query = @"SELECT pac_nombre, pac_apellido
                         FROM paciente
                         WHERE pac_id = @CodigoPaciente";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    // Asegúrate de usar Trim() para eliminar espacios adicionales
                    cmd.Parameters.AddWithValue("@CodigoPaciente", textBoxCodigoPaciente.Text.Trim());

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Concatenar nombre y apellido
                            string nombreCompleto = reader["pac_nombre"].ToString() + " " + reader["pac_apellido"].ToString();
                            textBoxNombreCompleto.Text = nombreCompleto;
                        }
                        else
                        {
                            textBoxNombreCompleto.Text = "";
                            MessageBox.Show("No se encontró ningún paciente con el código especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
            }
            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }
        public void guardarHistorialMedico(TextBox idPaciente, TextBox pacNombre, TextBox enfermedadNombre, DateTimePicker fechaConsulta, RichTextBox diagnostico, RichTextBox tratamiento)
        {
            // Obtener el ID del paciente desde el TextBox
            int pacId;

            if (!int.TryParse(idPaciente.Text, out pacId))
            {
                MessageBox.Show("ID del paciente inválido.");
                return;
            }

            // Validar que el nombre del paciente no esté vacío
            if (string.IsNullOrWhiteSpace(pacNombre.Text))
            {
                MessageBox.Show("El nombre del paciente no puede estar vacío.");
                return;
            }

            // Validar que el nombre de la enfermedad no esté vacío
            if (string.IsNullOrWhiteSpace(enfermedadNombre.Text))
            {
                MessageBox.Show("El nombre de la enfermedad no puede estar vacío.");
                return;
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Insertar en la tabla HistorialMedico
                string query = "INSERT INTO HistorialMedico(pac_id, pac_nombre, fecha_consulta, enfermedad_nombre, diagnostico, tratamiento) " +
                               "VALUES (@pacId, @pacNombre, @fechaConsulta, @enfermedadNombre, @diagnostico, @tratamiento)";

                using (MySqlCommand myCommandHistorial = new MySqlCommand(query, conexion))
                {
                    myCommandHistorial.Parameters.AddWithValue("@pacId", pacId);
                    myCommandHistorial.Parameters.AddWithValue("@pacNombre", pacNombre.Text);
                    myCommandHistorial.Parameters.AddWithValue("@fechaConsulta", fechaConsulta.Value.ToString("yyyy-MM-dd"));
                    myCommandHistorial.Parameters.AddWithValue("@enfermedadNombre", enfermedadNombre.Text);
                    myCommandHistorial.Parameters.AddWithValue("@diagnostico", diagnostico.Text);
                    myCommandHistorial.Parameters.AddWithValue("@tratamiento", tratamiento.Text);

                    myCommandHistorial.ExecuteNonQuery();
                    MessageBox.Show("Se guardó el historial médico");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el historial médico: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void BuscarHistorialPorFiltros(DataGridView tablaHistorial, TextBox textBoxFiltro, ComboBox comboBoxFiltro, DateTimePicker dateTimePickerFiltro)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Construir la consulta base
                string query = "SELECT id_historial as 'ID Historial', " +
                               "pac_nombre as 'Nombre Completo', " +
                               "fecha_consulta as 'Fecha de Consulta', " +
                               "enfermedad_nombre as 'Nombre de la Enfermedad', " +
                               "diagnostico as 'Diagnóstico', " +
                               "tratamiento as 'Tratamiento' " +
                               "FROM HistorialMedico WHERE 1=1";

                MySqlCommand command = new MySqlCommand(query, conexion);

                // Determinar el filtro basado en la selección del ComboBox
                if (comboBoxFiltro.SelectedItem != null)
                {
                    string filtroSeleccionado = comboBoxFiltro.SelectedItem.ToString();

                    // Filtro por "Nombre Completo"
                    if (filtroSeleccionado == "Nombre Completo" && !string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                    {
                        query += " AND pac_nombre LIKE @filtroNombre";
                        command.Parameters.AddWithValue("@filtroNombre", "%" + textBoxFiltro.Text.Trim() + "%");
                    }
                    // Filtro por "Fecha de Consulta"
                    else if (filtroSeleccionado == "Fecha de Consulta")
                    {
                        query += " AND DATE(fecha_consulta) = @filtroFecha";
                        DateTime fechaConsulta = dateTimePickerFiltro.Value.Date;
                        command.Parameters.AddWithValue("@filtroFecha", fechaConsulta.ToString("yyyy-MM-dd")); // Asegúrate del formato correcto
                    }
                }

                // Asignar la consulta final al comando
                command.CommandText = query;

                // Adaptador para llenar los datos en un DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Limpiar el DataGridView antes de mostrar los resultados
                tablaHistorial.DataSource = null;
                tablaHistorial.Rows.Clear();

                // Asignar el DataTable con los resultados al DataGridView
                tablaHistorial.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar historial médico: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }












    }
}
