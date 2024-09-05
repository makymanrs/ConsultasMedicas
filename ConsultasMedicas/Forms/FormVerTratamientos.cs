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

namespace ConsultasMedicas.Forms
{
    public partial class FormVerTratamientos : Form
    {
        private FormHistorialMedico formHistorialmedico;
        private FormEditarHistorialMedico formEditarHistorialMedico;
        public FormVerTratamientos()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.mostrarTratamientosPorEnfermedad(dataGridTratamiento);

        }
        public FormVerTratamientos(FormHistorialMedico formHistorialmedico = null, string nombreEnfermedad="")
        {
            InitializeComponent();
            ConfigurarDataGridView();
            textBox1.Text = nombreEnfermedad;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.mostrarTratamientosPorEnfermedad(dataGridTratamiento);
            this.formHistorialmedico = formHistorialmedico; 
        }
        public FormVerTratamientos(FormEditarHistorialMedico formEditarHistorialMedico = null, string nombreEnfermedad = "")
        {
            InitializeComponent();
            ConfigurarDataGridView();
            textBox1.Text = nombreEnfermedad;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.mostrarTratamientosPorEnfermedad(dataGridTratamiento);
            this.formEditarHistorialMedico = formEditarHistorialMedico;
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

        private void ConfigurarDataGridView()
        {
            foreach (DataGridViewColumn column in dataGridTratamiento.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0); // Ajusta según lo necesario
            }
            dataGridTratamiento.RowTemplate.Height = 60;
            dataGridTratamiento.ReadOnly = true;
            dataGridTratamiento.BorderStyle = BorderStyle.FixedSingle;
            dataGridTratamiento.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        public string nombreEnfermedades
        {
            set { textBox1.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormDetalleEnfermedades mostrarForm = new FormDetalleEnfermedades(this);
            mostrarForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.buscarTratamientosPorNombreEnfermedad(dataGridTratamiento, textBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mysql.Ctratamiento objetoTratamiento = new Mysql.Ctratamiento();
            objetoTratamiento.eliminarTratamiento(dataGridTratamiento);
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.buscarTratamientosPorNombreEnfermedad(dataGridTratamiento, textBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridTratamiento.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener los datos de la fila seleccionada
                int idTratamiento = Convert.ToInt32(dataGridTratamiento.SelectedRows[0].Cells["ID Tratamiento"].Value);
                string medicamento = dataGridTratamiento.SelectedRows[0].Cells["medicamento"].Value.ToString();
                string dosis = dataGridTratamiento.SelectedRows[0].Cells["dosis"].Value.ToString();
                string duracion = dataGridTratamiento.SelectedRows[0].Cells["duración"].Value.ToString();
                string observaciones = dataGridTratamiento.SelectedRows[0].Cells["observaciones"].Value.ToString();

                // Crear el formulario de edición con los datos obtenidos
                FormEditarTratamiento mostrarForm = new FormEditarTratamiento(idTratamiento, medicamento, dosis, duracion, observaciones);
                mostrarForm.ShowDialog();

                // Opcional: refrescar el DataGridView después de editar
                button2_Click(sender, e); // Asumiendo que button2_Click refresca el DataGridView
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para editar.");
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            button5.BackColor = Color.Red;

        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridTratamiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridTratamiento.Rows.Count)
            {
                // Verificar si FormHistorialMedico fue pasado
                if (formHistorialmedico != null)
                {
                    // Crear un StringBuilder para concatenar las observaciones
                    StringBuilder sb = new StringBuilder();

                    // Iterar sobre todas las filas seleccionadas
                    foreach (DataGridViewRow row in dataGridTratamiento.SelectedRows)
                    {
                        // Obtener la observación de la fila seleccionada
                        string observaciones = row.Cells["medicamento"].Value.ToString();

                        // Concatenar la observación con el símbolo y un salto de línea
                        sb.AppendLine($"• {observaciones}");
                    }

                    // Obtener el contenido actual del RichTextBox
                    string contenidoActual = formHistorialmedico.trataminetos;

                    // Añadir las nuevas observaciones al contenido actual
                    if (!string.IsNullOrEmpty(contenidoActual))
                    {
                        // Añadir una sola línea de separación entre el contenido actual y el nuevo contenido
                        formHistorialmedico.trataminetos = contenidoActual.TrimEnd() + Environment.NewLine + sb.ToString();
                    }
                    else
                    {
                        formHistorialmedico.trataminetos = sb.ToString();
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                if (formEditarHistorialMedico != null)
                {
                    // Crear un StringBuilder para concatenar las observaciones
                    StringBuilder sb = new StringBuilder();

                    // Iterar sobre todas las filas seleccionadas
                    foreach (DataGridViewRow row in dataGridTratamiento.SelectedRows)
                    {
                        // Obtener la observación de la fila seleccionada
                        string observaciones = row.Cells["medicamento"].Value.ToString();

                        // Concatenar la observación con el símbolo y un salto de línea
                        sb.AppendLine($"• {observaciones}");
                    }

                    // Obtener el contenido actual del RichTextBox
                    string contenidoActual = formEditarHistorialMedico.trataminetos;

                    // Añadir las nuevas observaciones al contenido actual
                    if (!string.IsNullOrEmpty(contenidoActual))
                    {
                        // Añadir una sola línea de separación entre el contenido actual y el nuevo contenido
                        formEditarHistorialMedico.trataminetos = contenidoActual.TrimEnd() + Environment.NewLine + sb.ToString();
                    }
                    else
                    {
                        formEditarHistorialMedico.trataminetos = sb.ToString();
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
