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
    public partial class FormEditarPaciente : Form
    {
        // Define el delegado para el evento
        public delegate void PacienteModificadoEventHandler(object sender, EventArgs e);
        // Define el evento basado en el delegado
        public event PacienteModificadoEventHandler PacienteModificado;
        public FormEditarPaciente()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada

        }
        protected virtual void OnPacienteModificado(EventArgs e)
        {
            PacienteModificado?.Invoke(this, e);
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
            if (int.TryParse(textBox4.Text, out int searchValue))
            {
                // Imprimir en el log para depuración
                Console.WriteLine($"Buscando paciente con ID: {searchValue}");

                Mysql.Cpacientes objetoPacientes = new Mysql.Cpacientes();
                DataRow paciente = objetoPacientes.buscarPaciente(searchValue);

                if (paciente != null)
                {
                    // Asignar valores a los controles del formulario
                    maskedTextBox1.Text = paciente["pac_identidad"].ToString();
                    textBox1.Text = paciente["pac_nombre"].ToString();
                    textBox2.Text = paciente["pac_apellido"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(paciente["pac_fecha_nacimiento"]);
                    numericUpDown1.Value = Convert.ToInt32(paciente["pac_edad"]);
                    textBox3.Text = paciente["pac_direccion"].ToString();
                    maskedTextBox2.Text = paciente["pac_telefono"].ToString();
                }
                else
                {
                    MessageBox.Show("Paciente no encontrado.");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor numérico válido para buscar.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mysql.Cpacientes objetoPacientes = new Mysql.Cpacientes();
            objetoPacientes.modificarPaciente(textBox4,maskedTextBox1, textBox1, textBox2, dateTimePicker1, numericUpDown1, textBox3, maskedTextBox2);
            OnPacienteModificado(EventArgs.Empty);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaNacimiento = dateTimePicker1.Value;
            int edadCalculada = DateTime.Now.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Now.AddYears(-edadCalculada)) edadCalculada--;
            numericUpDown1.Value = edadCalculada;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.Red;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(33, 33, 33);
        }
    }
}
