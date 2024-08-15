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
                string queryUsuario = "SELECT COUNT(*) FROM usuario WHERE usu_nom LIKE @user AND usu_pass LIKE @password";
                MySqlCommand commandUsuario = new MySqlCommand(queryUsuario, conexion);
                commandUsuario.Parameters.AddWithValue("@user", Usuario.Text);
                commandUsuario.Parameters.AddWithValue("@password", Contra.Text);
                int countUsuario = Convert.ToInt32(commandUsuario.ExecuteScalar());

                if (countUsuario > 0)
                {
                    MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loginForm.Hide(); // Cerrar el formulario de inicio de sesión
                    MostrarMenu(); // Mostrar el FormMenu después del inicio de sesión exitoso
                }
                else
                {
                    MessageBox.Show("No existe este usuario, verificar datos correctos.", "Datos errados", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    // Limpiar los campos de usuario y contraseña para volver a intentar
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

        private void MostrarMenu()
        {
            // mostrar Menu despues de logearse
            FormMenu form1 = new FormMenu();
            form1.Show();
        }
    }
}
