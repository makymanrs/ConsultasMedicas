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

        public FormEditarPaciente(int id, string nombre, string apellido, string identidad, string fechaNacimiento, string edad, string direccion, string telefono)
        {
            InitializeComponent();

            // Establecer bordes redondeados
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Inicializar controles con los valores proporcionados
            textBox4.Text = id.ToString(); // Asumir que textBox1 es para ID y debe ser un texto
            textBox2.Text = apellido;
            textBox1.Text = nombre;
            maskedTextBox1.Text = identidad;

            // Asegurarse de que la fecha se parsea correctamente
            if (DateTime.TryParse(fechaNacimiento, out DateTime parsedDate))
            {
                dateTimePicker1.Value = parsedDate;
            }
            else
            {
                // Manejar la fecha inválida, si es necesario
            //    MessageBox.Show("Fecha de nacimiento no válida.");
            }

            // Usar numericUpDown para edad, que es un número entero
            if (int.TryParse(edad, out int parsedEdad))
            {
                numericUpDown1.Value = parsedEdad;
            }
            else
            {
                // Manejar edad inválida, si es necesario
              //  MessageBox.Show("Edad no válida.");
            }

            textBox3.Text = direccion;
            maskedTextBox2.Text = telefono;
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

        private void FormEditarPaciente_Load(object sender, EventArgs e)
        {
            textBox4.TabIndex = 0;
            textBox4.Focus();
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                textBox1.Focus();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                textBox2.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                dateTimePicker1.Focus();
            }
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                numericUpDown1.Focus();
            }
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                maskedTextBox2.Focus();
            }
        }
    }
}
