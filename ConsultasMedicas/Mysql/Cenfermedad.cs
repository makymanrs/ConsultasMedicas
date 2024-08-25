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
    internal class Cenfermedad
    {
        public void mostrarTratamientosPorEnfermedad(DataGridView tablaTratamientos)
        {
            MySqlConnection conexion = null;
            try
            {
                // Establecer la conexión con la base de datos
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Suponiendo que deseas buscar tratamientos para una enfermedad específica por nombre
                string nombreEnfermedad = "Nombre de la Enfermedad"; // Aquí puedes establecer el nombre de la enfermedad que quieres buscar

                // Consulta SQL para obtener los tratamientos asociados a una enfermedad específica
                string query = "SELECT t.id_tratamiento as 'ID', " +
                               "t.medicamento as 'Medicamento', " +
                               "t.dosis as 'Dosis', " +
                               "t.duracion as 'Duración', " +
                               "t.observaciones as 'Observaciones' " +
                               "FROM Tratamientos t " +
                               "INNER JOIN Enfermedades e ON t.id_enfermedad = e.id_enfermedad " +
                               "WHERE e.nombre = @nombreEnfermedad";

                // Crear el adaptador de datos y la tabla
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@nombreEnfermedad", nombreEnfermedad); // Pasar el parámetro de la enfermedad

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Asignar los datos al DataGridView
                tablaTratamientos.DataSource = dt;
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
        // busca los tratamientos en base a la nombre de la enfermedad como consulta
        public void buscarTratamientosPorNombreEnfermedad(DataGridView tablaTratamientos, TextBox nombreEnfermedad)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Query para buscar tratamientos por nombre de la enfermedad
                string query = "SELECT t.id_tratamiento as 'ID Tratamiento', " +
                               "t.medicamento as 'Medicamento', " +
                               "t.dosis as 'Dosis', " +
                               "t.duracion as 'Duración', " +
                               "t.observaciones as 'Observaciones' " +
                               "FROM Tratamientos t " +
                               "INNER JOIN Enfermedades e ON t.id_enfermedad = e.id_enfermedad " +
                               "WHERE e.nombre = @nombreEnfermedad";

                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@nombreEnfermedad", nombreEnfermedad.Text.Trim());

                // Adaptador para llenar los datos en un DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Limpiar el DataGridView antes de mostrar los resultados
                tablaTratamientos.DataSource = null;
                tablaTratamientos.Rows.Clear();

                // Asignar el DataTable con los resultados al DataGridView
                tablaTratamientos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar tratamientos: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        // guardar los datos 
        public void guardarEnfermedad(TextBox nombre, RichTextBox descripcion, RichTextBox sintomas)
        {
            // Verificar si alguno de los campos está vacío
            if (string.IsNullOrWhiteSpace(nombre.Text) || string.IsNullOrWhiteSpace(sintomas.Text) || string.IsNullOrWhiteSpace(descripcion.Text))
            {
                MessageBox.Show("Todos los campos deben estar llenos.");
                return;
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Convertir a minúsculas el nombre para la verificación
                string nombreEnfermedad = nombre.Text.ToLower();

                // Verificar si la enfermedad ya existe en la base de datos (sin importar mayúsculas o minúsculas)
                string queryVerificar = "SELECT COUNT(*) FROM Enfermedades WHERE LOWER(nombre) = @nombre";
                MySqlCommand comandoVerificar = new MySqlCommand(queryVerificar, conexion);
                comandoVerificar.Parameters.AddWithValue("@nombre", nombreEnfermedad);
                int count = Convert.ToInt32(comandoVerificar.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("La enfermedad ya está registrada.");
                    return;
                }

                // Insertar en la tabla enfermedades
                string queryInsertar = "INSERT INTO Enfermedades (nombre, descripcion, sintomas) " +
                                       "VALUES (@nombreOriginal, @descripcion, @sintomas)";
                MySqlCommand myCommandEnfermedad = new MySqlCommand(queryInsertar, conexion);
                myCommandEnfermedad.Parameters.AddWithValue("@nombreOriginal", nombre.Text);  // Usar el texto original para la inserción
                myCommandEnfermedad.Parameters.AddWithValue("@descripcion", descripcion.Text);
                myCommandEnfermedad.Parameters.AddWithValue("@sintomas", sintomas.Text);

                myCommandEnfermedad.ExecuteNonQuery();
                MessageBox.Show("Se guardó el registro de la enfermedad.");
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
        // muestra los detalles como los richtextbox 
        public void mostrarDetallesEnfermedad(TextBox nombreEnfermedad, RichTextBox descripcionEnfermedad, RichTextBox sintomasEnfermedad)
        {
            MySqlConnection conexion = null;
            try
            {
                // Establecer la conexión con la base de datos
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Consulta SQL para obtener los detalles de la enfermedad basada en el nombre
                string query = "SELECT descripcion, sintomas " +
                               "FROM Enfermedades " +
                               "WHERE nombre = @nombreEnfermedad";

                // Crear el adaptador de datos
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@nombreEnfermedad", nombreEnfermedad.Text.Trim());

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Verificar si se encontraron resultados
                if (dt.Rows.Count > 0)
                {
                    // Mostrar los detalles en los controles correspondientes
                    DataRow row = dt.Rows[0];
                    descripcionEnfermedad.Text = row["descripcion"].ToString();
                    sintomasEnfermedad.Text = row["sintomas"].ToString();
                }
                else
                {
                    // Limpiar los controles si no se encuentran resultados
                    descripcionEnfermedad.Clear();
                    sintomasEnfermedad.Clear();
                    MessageBox.Show("No se encontró la enfermedad.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("No se pudieron recuperar los datos: " + ex.ToString());
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
        // modificar o actualizar los datos 
        public void modificarEnfermedad(TextBox id, TextBox nombre, RichTextBox descripcion, RichTextBox sintomas)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Consulta SQL para actualizar el registro en la tabla Enfermedades
                string query = "UPDATE Enfermedades SET nombre = @nombre, descripcion = @descripcion, sintomas = @sintomas WHERE id_enfermedad = @id_enfermedad";
                MySqlCommand command = new MySqlCommand(query, conexion);

                // Asignar valores a los parámetros de la consulta
                command.Parameters.AddWithValue("@id_enfermedad", id.Text);
                command.Parameters.AddWithValue("@nombre", nombre.Text);
                command.Parameters.AddWithValue("@descripcion", descripcion.Text);
                command.Parameters.AddWithValue("@sintomas", sintomas.Text);

                // Ejecutar la consulta
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
        // busca las enfermedades solo con el nombre 
        public DataRow buscarEnfermedad(int id)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Consulta SQL ajustada a la tabla Enfermedades
                string query = "SELECT id_enfermedad, nombre, descripcion, sintomas " +
                               "FROM Enfermedades WHERE id_enfermedad = @idenfermedad";
                MySqlCommand command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@idenfermedad", id);

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
                MessageBox.Show("Error al buscar enfermedad: " + ex.Message);
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
        // elimina las enfermedades aun falta por mejorar
        public void eliminarEnfermedad(TextBox cod, DataGridView tablaEnfermedad)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Eliminar la enfermedad de la tabla Enfermedades
                string queryEnfermedad = "DELETE FROM Enfermedades WHERE id_enfermedad = @enfermedadId";
                MySqlCommand commandEnfermedad = new MySqlCommand(queryEnfermedad, conexion);
                commandEnfermedad.Parameters.AddWithValue("@enfermedadId", cod.Text);

                int rowsAffected = commandEnfermedad.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se eliminó el registro de la Enfermedad.");
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna Enfermedad con ese ID.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el registro de la Enfermedad. Error: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }

        // muestra las enfermedades ya registradas
        public void mostrarEnfermedades(DataGridView tablaEnfermedades)
        {
            MySqlConnection conexion = null;
            try
            {
                // Establecer la conexión con la base de datos
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Consulta SQL para obtener todas las enfermedades
                string query = "SELECT id_enfermedad AS 'ID', " +
                               "nombre AS 'Nombre', " +
                               "descripcion AS 'Descripción', " +
                               "sintomas AS 'Síntomas' " +
                               "FROM Enfermedades";

                // Crear el adaptador de datos y la tabla
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Asignar los datos al DataGridView
                tablaEnfermedades.DataSource = dt;
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
        // Busqueda de enfermedades por filtros
        public void BuscarEnfermedadesPorFiltros(DataGridView tablaEnfermedades, TextBox textBoxFiltro, ComboBox comboBoxFiltro)
        {
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Construir la consulta base
                string query = "SELECT id_enfermedad AS 'ID', nombre AS 'Nombre', descripcion AS 'Descripción', sintomas AS 'Síntomas' FROM Enfermedades WHERE 1=1";

                // Determinar el filtro basado en la selección del ComboBox
                if (comboBoxFiltro.SelectedItem.ToString() == "Nombre" && !string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    query += " AND nombre LIKE @filtro";
                }
                else if (comboBoxFiltro.SelectedItem.ToString() == "Código" && !string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    query += " AND id_enfermedad = @filtro";
                }

                MySqlCommand command = new MySqlCommand(query, conexion);

                // Asignar valores a los parámetros
                if (!string.IsNullOrWhiteSpace(textBoxFiltro.Text))
                {
                    if (comboBoxFiltro.SelectedItem.ToString() == "Nombre")
                    {
                        command.Parameters.AddWithValue("@filtro", "%" + textBoxFiltro.Text.Trim() + "%");
                    }
                    else if (comboBoxFiltro.SelectedItem.ToString() == "Código")
                    {
                        command.Parameters.AddWithValue("@filtro", textBoxFiltro.Text.Trim());
                    }
                }

                // Adaptador para llenar los datos en un DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Limpiar el DataGridView antes de mostrar los resultados
                tablaEnfermedades.DataSource = null;
                tablaEnfermedades.Rows.Clear();

                // Asignar el DataTable con los resultados al DataGridView
                tablaEnfermedades.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar enfermedad: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        public void seleccionarEnfermedad(DataGridView tablaDetalle, TextBox textboxdetalleoId)
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
        public List<string> ObtenerNombresEnfermedades()
        {
            List<string> nombresEnfermedades = new List<string>();
            MySqlConnection conexion = null;

            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                string query = "SELECT nombre FROM Enfermedades";
                MySqlCommand command = new MySqlCommand(query, conexion);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nombresEnfermedades.Add(reader["nombre"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener nombres de enfermedades: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }

            return nombresEnfermedades;
        }
    }
}
