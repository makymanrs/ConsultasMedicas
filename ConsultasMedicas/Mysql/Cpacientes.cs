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
    internal class Cpacientes
    {
        public void mostrarPaciente(DataGridView tablaPaciente)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();
                string query = "SELECT pac_id as 'ID', " +
                               "CONCAT(pac_nombre, ' ', pac_apellido) as 'Nombre Completo', " +
                               "pac_identidad as 'Identidad', "+
                               "pac_fecha_nacimiento as 'Fecha de Nacimiento', " +
                               "pac_edad as 'Edad', " +
                               "pac_direccion as 'Dirección', pac_telefono as 'Teléfono' " +
                               "FROM paciente";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                tablaPaciente.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encontraron los datos de la base de datos, error: " + ex.ToString());
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }

        public void guardarPaciente(MaskedTextBox identidad, TextBox nombre, TextBox apellido, DateTimePicker fechaNacimiento, NumericUpDown edad, TextBox direccion, MaskedTextBox telefono)
        {
            // Verificar si alguno de los campos está vacío
            if (string.IsNullOrWhiteSpace(nombre.Text) || string.IsNullOrWhiteSpace(apellido.Text) ||
                string.IsNullOrWhiteSpace(direccion.Text) || string.IsNullOrWhiteSpace(telefono.Text) || string.IsNullOrWhiteSpace(identidad.Text))
            {
                MessageBox.Show("Todos los campos deben estar llenos.");
                return;
            }

            // Calcular la edad en base a la fecha de nacimiento
            int edadCalculada = DateTime.Now.Year - fechaNacimiento.Value.Year;
            if (fechaNacimiento.Value.Date > DateTime.Now.AddYears(-edadCalculada)) edadCalculada--;

            // Asignar la edad calculada al NumericUpDown
            edad.Value = edadCalculada;

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Insertar en la tabla paciente
                string query = "INSERT INTO paciente(pac_identidad, pac_nombre, pac_apellido, pac_fecha_nacimiento, pac_edad, pac_direccion, pac_telefono) " +
                               "VALUES (@pacidentidad, @pacnombre, @pacapellido, @pacfechanacimiento, @pacedad, @pacdireccion, @pactelefono)";
                MySqlCommand myCommandPaciente = new MySqlCommand(query, conexion);
                myCommandPaciente.Parameters.AddWithValue("@pacidentidad", identidad.Text);
                myCommandPaciente.Parameters.AddWithValue("@pacnombre", nombre.Text);
                myCommandPaciente.Parameters.AddWithValue("@pacapellido", apellido.Text);
                myCommandPaciente.Parameters.AddWithValue("@pacfechanacimiento", fechaNacimiento.Value.ToString("yyyy-MM-dd"));
                myCommandPaciente.Parameters.AddWithValue("@pacedad", edadCalculada); // Edad calculada
                myCommandPaciente.Parameters.AddWithValue("@pacdireccion", direccion.Text);
                myCommandPaciente.Parameters.AddWithValue("@pactelefono", telefono.Text);

                myCommandPaciente.ExecuteNonQuery();
                MessageBox.Show("Se guardó el registro");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el registro: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public DataRow buscarPaciente(int id)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Consulta SQL ajustada a la tabla paciente
                string query = "SELECT pac_id, pac_identidad, pac_nombre, pac_apellido, pac_fecha_nacimiento, pac_edad, pac_direccion, pac_telefono " +
                               "FROM paciente WHERE pac_id = @pacid";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@pacid", id);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]; // Devuelve la primera fila encontrada
                }
                else
                {
                    return null; // No se encontraron resultados
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
                return null;
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }

        public void modificarPaciente(TextBox cod, MaskedTextBox identidad, TextBox nombre, TextBox apellido, DateTimePicker fechaNacimiento,NumericUpDown edad, TextBox direccion, MaskedTextBox telefono)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Calcular la edad en base a la fecha de nacimiento
                int edadCalculada = DateTime.Now.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value.Date > DateTime.Now.AddYears(-edadCalculada)) edadCalculada--;

                string query = "UPDATE paciente SET pac_identidad=@pacidentidad, pac_nombre=@pacnombre, pac_apellido=@pacapellido, pac_fecha_nacimiento=@pacfechanacimiento, pac_edad=@pacedad, pac_direccion=@pacdireccion, pac_telefono=@pactelefono WHERE pac_id = @pacid";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@pacid", cod.Text);
                command.Parameters.AddWithValue("@pacidentidad", identidad.Text);
                command.Parameters.AddWithValue("@pacnombre", nombre.Text);
                command.Parameters.AddWithValue("@pacapellido", apellido.Text);
                command.Parameters.AddWithValue("@pacfechanacimiento", fechaNacimiento.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@pacedad", edadCalculada); // Edad calculada
                command.Parameters.AddWithValue("@pacdireccion", direccion.Text);
                command.Parameters.AddWithValue("@pactelefono", telefono.Text);

                command.ExecuteNonQuery();
                MessageBox.Show("Se modificaron los registros correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo modificar el registro: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void eliminarPaciente(DataGridView dataGridPaciente)
        {
            // Verificar si hay una fila seleccionada
            if (dataGridPaciente.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un paciente para eliminar.");
                return;
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Obtener el ID del paciente de la fila seleccionada
                int idPaciente = Convert.ToInt32(dataGridPaciente.SelectedRows[0].Cells["ID"].Value);

                // Eliminar el paciente de la tabla paciente
                string queryPaciente = "DELETE FROM paciente WHERE pac_id = @pacid";
                MySqlCommand commandPaciente = new MySqlCommand(queryPaciente, conexion);
                commandPaciente.Parameters.AddWithValue("@pacid", idPaciente);

                int rowsAffected = commandPaciente.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se eliminó el registro del paciente.");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún paciente con ese ID.");
                }

                // Opcional: Actualizar el DataGridView después de la eliminación
                //MostrarPacientes(dataGridPaciente); // Asegúrate de tener este método en tu clase Cpacientes
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el registro del paciente. Error: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void BuscarPacientesPorFiltros(DataGridView tablaPacientes, TextBox textBoxFiltro, ComboBox comboBoxFiltro)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Construir la consulta base
                string query = "SELECT pac_id as 'ID', " +  // Se añade la coma aquí
                 "CONCAT(pac_nombre, ' ', pac_apellido) AS 'Nombre Completo', " +
                 "pac_identidad as 'Identidad', " +
                 "pac_fecha_nacimiento as 'Fecha de Nacimiento', pac_edad as 'Edad', " +
                 "pac_direccion as 'Dirección', pac_telefono as 'Teléfono' " +
                 "FROM paciente WHERE 1=1";


                // Determinar el filtro basado en la selección del ComboBox
                if (comboBoxFiltro.SelectedItem.ToString() == "Nombre Completo" && !string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    query += " AND CONCAT(pac_nombre, ' ', pac_apellido) LIKE @filtro";
                }
                else if (comboBoxFiltro.SelectedItem.ToString() == "ID" && !string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    query += " AND pac_id = @filtro";
                }

                MySqlCommand command = new MySqlCommand(query, conexion);

                // Asignar valores a los parámetros
                if (!string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    if (comboBoxFiltro.SelectedItem.ToString() == "Nombre Completo")
                    {
                        command.Parameters.AddWithValue("@filtro", "%" + textBoxFiltro.Text.Trim() + "%");
                    }
                    else if (comboBoxFiltro.SelectedItem.ToString() == "ID")
                    {
                        command.Parameters.AddWithValue("@filtro", textBoxFiltro.Text.Trim());
                    }
                }

                // Adaptador para llenar los datos en un DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Limpiar el DataGridView antes de mostrar los resultados
                tablaPacientes.DataSource = null;
                tablaPacientes.Rows.Clear();

                // Asignar el DataTable con los resultados al DataGridView
                tablaPacientes.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void seleccionarPaciente(DataGridView tablaDetalle, TextBox textboxdetalleoId)
        {
            try
            {
                // Obtener el índice de la columna seleccionada
                int columnIndex = tablaDetalle.CurrentCell.ColumnIndex;
                if (columnIndex == 0)
                {
                    textboxdetalleoId.Text = tablaDetalle.CurrentRow.Cells[0].Value.ToString();
                }
                else if (columnIndex == 1)
                {
                    textboxdetalleoId.Text = tablaDetalle.CurrentRow.Cells[1].Value.ToString();
                }
                else
                {
                    // Puedes manejar el caso cuando no es ninguna de las columnas deseadas
                    MessageBox.Show("No se seleccionó una columna válida.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logró seleccionar, error: " + ex.ToString());
            }
        }
        public List<string> ObtenerNombrePacientes()
        {
            List<string> nombresPacientes = new List<string>();
            using (MySqlConnection conexion = new Conexion().establecerConexion())
            {
                try
                {
                    // Consulta SQL para concatenar pac_nombre y pac_apellido
                    string query = "SELECT CONCAT(pac_nombre, ' ', pac_apellido) AS NombreCompleto FROM paciente";
                    MySqlCommand command = new MySqlCommand(query, conexion);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Agregar el nombre completo a la lista
                        nombresPacientes.Add(reader["NombreCompleto"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener nombres de pacientes: " + ex.Message);
                }
            }
            return nombresPacientes;
        }

    }
}
