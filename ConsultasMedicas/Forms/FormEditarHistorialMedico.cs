using ConsultasMedicas.Mysql;
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
    public partial class FormEditarHistorialMedico : Form
    {
        public FormEditarHistorialMedico(int idHistorial, string nombrePaciente, DateTime fechaConsulta, string nombreEnfermedad, string diagnostico, string tratamiento)
        {
            InitializeComponent();
            // Asignar los valores recibidos a los controles del formulario
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            textBox1.Text = idHistorial.ToString();
            textBox2.Text = nombrePaciente;
            dateTimePicker1.Value = fechaConsulta;
            textBox3.Text = nombreEnfermedad;
            richTextBox1.Text = diagnostico;
            richTextBox2.Text = tratamiento;
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

        public Action OnDataUpdated { get; internal set; }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int searchValue))
            {
                // Imprimir en el log para depuración
                Console.WriteLine($"Buscando historial médico con ID: {searchValue}");

                // Instancia del objeto que contiene el método buscarHistorialMedico
                Mysql.Chistorialmedico objetoHistorialMedico = new Mysql.Chistorialmedico();
                DataRow historialMedico = objetoHistorialMedico.buscarHistorialMedico(searchValue);

                if (historialMedico != null)
                {
                    // Asignar valores a los controles del formulario con los datos obtenidos
                    textBox1.Text = historialMedico["id_historial"].ToString();
                    textBox2.Text = historialMedico["pac_nombre"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(historialMedico["fecha_consulta"]);
                    textBox3.Text = historialMedico["enfermedad_nombre"].ToString();
                    richTextBox1.Text = historialMedico["diagnostico"].ToString();
                    richTextBox2.Text = historialMedico["tratamiento"].ToString();
                }
                else
                {
                    MessageBox.Show("Historial médico no encontrado.");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor numérico válido para buscar.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Mysql.Chistorialmedico objetoHistorialMedico = new Mysql.Chistorialmedico();
            objetoHistorialMedico.modificarHistorialMedico(textBox1, textBox2, dateTimePicker1, textBox3, richTextBox1, richTextBox2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.Red;
        }
        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(33, 33, 33);
        }
        public string NombreEnfermedad
        {
            set { textBox3.Text = value; } // Asumiendo que textBox2 es el TextBox para el nombre
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string nombreEnfermedad = textBox3.Text;
            FormVerTratamientos mostrarForm = new FormVerTratamientos(this, nombreEnfermedad);
            mostrarForm.ShowDialog();
        }
        private string _tratamientos;
        public string trataminetos
        {
            get { return _tratamientos; }
            set { _tratamientos = value; richTextBox2.Text = _tratamientos; }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FormDetalleEnfermedades mostrarForm = new FormDetalleEnfermedades(this);
            mostrarForm.ShowDialog();
        }
    }
}
