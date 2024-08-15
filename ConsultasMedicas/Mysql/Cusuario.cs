using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas.Mysql
{
    internal class Cusuario
    {
        // metodo login
        public void inicioSesion(TextBox Usuario, TextBox Contra, Form loginForm)
        {
            if (string.IsNullOrEmpty(Usuario.Text) || string.IsNullOrEmpty(Contra.Text))
            {
                MessageBox.Show("Favor llenar todos los campos", "Campos vacíos!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Salir del método si hay campos vacíos
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();
                string queryUsuario = "SELECT COUNT(*), usu_nom FROM usuario WHERE usu_nom = @user AND usu_pass = @password";
                MySqlCommand commandUsuario = new MySqlCommand(queryUsuario, conexion);
                commandUsuario.Parameters.AddWithValue("@user", Usuario.Text);
                commandUsuario.Parameters.AddWithValue("@password", Contra.Text);
                MySqlDataReader reader = commandUsuario.ExecuteReader();

                if (reader.Read() && reader.GetInt32(0) > 0)
                {
                    string nombreUsuario = reader.GetString(1);
                    MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loginForm.Hide(); // Cierra el formulario de inicio de sesión
                    MostrarMenu(nombreUsuario); // Mostrar el FormMenu después del inicio de sesión exitoso
                }
                else
                {
                    MessageBox.Show("No existe este usuario, verificar datos correctos.", "Datos errados", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    // Limpia los campos de usuario y contraseña para volver a intentar
                    Usuario.Text = "";
                    Contra.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logró el acceso, intente en un momento: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }

        //metodo registro 
        public bool RegistrarUsuario(TextBox Usuario, TextBox Contra, TextBox confirmarContra)
        {
            if (string.IsNullOrEmpty(Usuario.Text) || string.IsNullOrEmpty(Contra.Text) || string.IsNullOrEmpty(confirmarContra.Text))
            {
                MessageBox.Show("Favor llenar todos los campos", "Campos vacíos!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.Compare(Contra.Text, confirmarContra.Text) != 0)
            {
                MessageBox.Show("Ambas contraseñas deben ser IGUALES, favor intente nuevamente", "Contraseñas Diferentes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Contra.Text = "";
                confirmarContra.Text = "";
                Contra.Focus();
                return false;
            }

            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                string queryVerificarUsuario = "SELECT COUNT(*) FROM usuario WHERE usu_nom LIKE @usu;";
                MySqlCommand commandVerificarUsuario = new MySqlCommand(queryVerificarUsuario, conexion);
                commandVerificarUsuario.Parameters.AddWithValue("@usu", Usuario.Text);
                int countUsuario = Convert.ToInt32(commandVerificarUsuario.ExecuteScalar());

                if (countUsuario > 0)
                {
                    MessageBox.Show("Ya existe un Usuario con el mismo nombre. Intente uno nuevo", "Duplicación de Usuario", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }

                string query = "INSERT INTO usuario (usu_nom, usu_pass) VALUES (@usu_nom, @usu_password)";
                MySqlCommand myCommandCliente = new MySqlCommand(query, conexion);
                myCommandCliente.Parameters.AddWithValue("@usu_nom", Usuario.Text);
                myCommandCliente.Parameters.AddWithValue("@usu_password", confirmarContra.Text);
                myCommandCliente.ExecuteNonQuery();
                MessageBox.Show("Usuario registrado con éxito", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpia campos después del registro exitoso si es necesario
                Usuario.Text = "";
                Contra.Text = "";
                confirmarContra.Text = "";

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el registro: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }
        private void MostrarMenu(string nombreUsuario)
        {
            // Mostrar Menu después de logearse
            FormMenu form1 = new FormMenu(nombreUsuario);
            form1.Show();
        }
    }
}
