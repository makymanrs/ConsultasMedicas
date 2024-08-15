using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada

        }
        // Esto es para las esquinas redondeadas no es necesario modificar nada solo asegurese de incorporarlos en los formularios
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
      (
          int nLeftRect,     // x-coordinate of upper-left corner
          int nTopRect,      // y-coordinate of upper-left corner
          int nRightRect,    // x-coordinate of lower-right corner
          int nBottomRect,   // y-coordinate of lower-right corner
          int nWidthEllipse, // height of ellipse
          int nHeightEllipse // width of ellipse
      );
        // este es para arrastrar y mover 
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //evento para arrastrar y mover el panel 1
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //es el hover al pasar el cursor al boton cerrar
        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;
        }

        //vuelve a su estado original el button1 que es cerrar
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(33, 33, 33);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Mysql.Cusuario cusuario = new Mysql.Cusuario();
            cusuario.inicioSesion(textBox1, textBox2, this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mysql.Conexion objetoConexion = new Mysql.Conexion();
            objetoConexion.establecerConexion();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide(); // Oculta el formulario de login
            RegistroLogin form2 = new RegistroLogin();
            form2.ShowDialog(); // Muestra el formulario de registro como un diálogo modal
            //this.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }
    }
}
