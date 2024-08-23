using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConsultasMedicas.Forms
{
    public partial class FormEditarEnfermedad : Form
    {
        public FormEditarEnfermedad()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Obtener el área del formulario
            Graphics g = e.Graphics;

            // Configura el color y el grosor del borde
            Pen borderPen = new Pen(Color.Black, 8); // Puedes ajustar el color y grosor aquí

            // Dibujar el borde alrededor del formulario
            g.DrawPath(borderPen, CreateRoundRectPath(0, 0, Width, Height, 20, 20)); // El grosor del borde es 2 y el radio de las esquinas es 20
        }

        private GraphicsPath CreateRoundRectPath(int x, int y, int width, int height, int radiusX, int radiusY)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radiusX, radiusY, 180, 90);
            path.AddArc(x + width - radiusX, y, radiusX, radiusY, 270, 90);
            path.AddArc(x + width - radiusX, y + height - radiusY, radiusX, radiusY, 0, 90);
            path.AddArc(x, y + height - radiusY, radiusX, radiusY, 90, 90);
            path.CloseFigure();
            return path;
        }

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
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.modificarEnfermedad(textBox2,textBox1,richTextBox1,richTextBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int searchValue))
            {
                // Imprimir en el log para depuración
                Console.WriteLine($"Buscando enfermedad con ID: {searchValue}");

                Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
                DataRow enfermedad = objetoEnfermedad.buscarEnfermedad(searchValue);

                if (enfermedad != null)
                {
                    // Asignar valores a los controles del formulario
                    textBox1.Text = enfermedad["nombre"].ToString();
                    richTextBox1.Text = enfermedad["descripcion"].ToString();
                    richTextBox2.Text = enfermedad["sintomas"].ToString();
                }
                else
                {
                    MessageBox.Show("Enfermedad no encontrada.");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor numérico válido para buscar.");
            }
        }
    }
}
