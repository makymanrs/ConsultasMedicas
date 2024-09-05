using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsultasMedicas.Forms
{
    public partial class FormDetalleEnfermedades : Form
    {
        private FormTratamiento formTratamiento;
        private FormVerTratamientos formVerTratamientos;
        private FormEnfermedades formEnfermedades;
        private FormHistorialMedico formHistorialMedico;
        private FormEditarHistorialMedico formEditarHistorialMedico;

        public FormDetalleEnfermedades(FormTratamiento formTratamiento = null)
        {
            InitializeComponent();
            // Establece la región redondeada inicial
            this.formTratamiento = formTratamiento;
            ApplyRoundRectangleRegion();
            // Carga los datos en el DataGridView
            LoadData();
            // Configura el evento de redimensionamiento
            this.Resize += FormDetalleEnfermedades_Resize;
            ConfigurarAutocompletado(textBox1);
        }
        public FormDetalleEnfermedades(FormVerTratamientos formVerTratamientos = null)
        {
            InitializeComponent();
            // Establece la región redondeada inicial
            this.formVerTratamientos = formVerTratamientos;
            ApplyRoundRectangleRegion();
            // Carga los datos en el DataGridView
            LoadData();
            // Configura el evento de redimensionamiento
            this.Resize += FormDetalleEnfermedades_Resize;
            ConfigurarAutocompletado(textBox1);

        }
        public FormDetalleEnfermedades(FormHistorialMedico formHistorialMedico = null)
        {
            InitializeComponent();
            // Establece la región redondeada inicial
            this.formHistorialMedico = formHistorialMedico;
            ApplyRoundRectangleRegion();
            // Carga los datos en el DataGridView
            LoadData();
            // Configura el evento de redimensionamiento
            this.Resize += FormDetalleEnfermedades_Resize;
            ConfigurarAutocompletado(textBox1);
        }
        public FormDetalleEnfermedades(FormEditarHistorialMedico formEditarHistorialMedico = null)
        {
            InitializeComponent();
            // Establece la región redondeada inicial
            this.formEditarHistorialMedico = formEditarHistorialMedico;
            ApplyRoundRectangleRegion();
            // Carga los datos en el DataGridView
            LoadData();
            // Configura el evento de redimensionamiento
            this.Resize += FormDetalleEnfermedades_Resize;
            ConfigurarAutocompletado(textBox1);
        }

        public FormDetalleEnfermedades(FormEnfermedades formEnfermedades)
        {
            this.formEnfermedades = formEnfermedades;
        }

        public FormDetalleEnfermedades()
        {
            InitializeComponent();
            ApplyRoundRectangleRegion();
            // Carga los datos en el DataGridView
            LoadData();
            // Configura el evento de redimensionamiento
            this.Resize += FormDetalleEnfermedades_Resize;
            ConfigurarAutocompletado(textBox1);

        }

        public void lista()
        {
            comboBox1.Items.Add("Ninguno");
            comboBox1.Items.Add("Código");
            comboBox1.Items.Add("Nombre");
            comboBox1.SelectedIndex = 0;
        }

        private void FormDetalleEnfermedades_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.GetWorkingArea(this);
            lista();
        }

        private void ApplyRoundRectangleRegion()
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                this.Invalidate(); // Forza el redibujo del formulario para aplicar la región
            }
        }

        private void LoadData()
        {
            Mysql.Cenfermedad objetioEnfermedad = new Mysql.Cenfermedad();
            objetioEnfermedad.mostrarEnfermedades(dataGridDetalleEnfermedad);
            foreach (DataGridViewColumn column in dataGridDetalleEnfermedad.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0); // Ajusta según lo necesario
            }
            dataGridDetalleEnfermedad.RowTemplate.Height = 60;
            dataGridDetalleEnfermedad.ReadOnly = true;

            dataGridDetalleEnfermedad.BorderStyle = BorderStyle.FixedSingle;
            dataGridDetalleEnfermedad.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.WindowState != FormWindowState.Maximized)
            {
                // Obtener el área del formulario
                Graphics g = e.Graphics;

                // Configura el color y el grosor del borde
                Pen borderPen = new Pen(Color.Black, 8); // Puedes ajustar el color y grosor aquí

                // Dibujar el borde alrededor del formulario
                g.DrawPath(borderPen, CreateRoundRectPath(0, 0, Width, Height, 20, 20));
            }
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
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);



        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

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

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridDetalleEnfermedad.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener los datos de la fila seleccionada
                int idEnfermedad = Convert.ToInt32(dataGridDetalleEnfermedad.SelectedRows[0].Cells["ID"].Value);
                string nombreEnfermedad = dataGridDetalleEnfermedad.SelectedRows[0].Cells["Nombre"].Value?.ToString() ?? string.Empty;
                string descripcionEnfermedad = dataGridDetalleEnfermedad.SelectedRows[0].Cells["Descripción"].Value?.ToString() ?? string.Empty;
                string sintomasEnfermedad = dataGridDetalleEnfermedad.SelectedRows[0].Cells["Síntomas"].Value?.ToString() ?? string.Empty;

                // Crear el formulario de edición con los datos obtenidos
                FormEditarEnfermedad formEditar = new FormEditarEnfermedad(idEnfermedad, nombreEnfermedad, descripcionEnfermedad, sintomasEnfermedad);

                formEditar.OnDataUpdated += () =>
                {
                    // Actualiza los datos después de editar
                    LoadData();
                };
                formEditar.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para editar.");
            }
            ConfigurarAutocompletado(textBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void FormDetalleEnfermedades_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Region = null; // Elimina la región redondeada al maximizar
                this.Invalidate(); // Forza el redibujo del formulario para eliminar el borde
            }
            else
            {
                ApplyRoundRectangleRegion(); // Aplica la región redondeada al restaurar el tamaño
            }
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(66, 66, 66);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.BuscarEnfermedadesPorFiltros(dataGridDetalleEnfermedad, textBox1, comboBox1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetioEnfermedad = new Mysql.Cenfermedad();
            objetioEnfermedad.mostrarEnfermedades(dataGridDetalleEnfermedad);
            textBox1.Text = "";
        }

        private void dataGridDetalleEnfermedad_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.seleccionarEnfermedad(dataGridDetalleEnfermedad, textBox1);
        }

        private void dataGridDetalleEnfermedad_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView != null)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                if (row.Selected)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.Font = new Font(dataGridView.Font.FontFamily, 11, FontStyle.Bold); // Cambia el tamaño de letra a 12 y lo pone en negrita
                        cell.Style.ForeColor = Color.Black;
                    }
                }
                else
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.Font = new Font(dataGridView.Font.FontFamily, 10, FontStyle.Regular); // Restablece el tamaño de letra a 10 y quita la negrita
                        cell.Style.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void dataGridDetalleEnfermedad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no se haya hecho clic en el encabezado de columna o fuera del rango de celdas
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0 && e.RowIndex < dataGridDetalleEnfermedad.Rows.Count)
            {
                // Verificar si la fila seleccionada no es nula o no contiene datos vacíos
                var row = dataGridDetalleEnfermedad.Rows[e.RowIndex];
                if (row.Cells[0].Value == null || string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    MessageBox.Show("No hay ningún dato en esta fila", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si FormDetalleEnfermedades fue abierto desde FormTratamiento
                if (formTratamiento != null)
                {
                    // Obtener el código seleccionado
                    string codigoSeleccionado = row.Cells[0].Value.ToString();

                    // Pasar el código seleccionado a FormTratamiento
                    formTratamiento.CodigoEnfermedad = codigoSeleccionado;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                // Verificar si FormDetalleEnfermedades fue abierto desde FormVerTratamientos
                if (formVerTratamientos != null)
                {
                    string nombreSeleccionado = row.Cells["Nombre"].Value.ToString();

                    // Asegúrate de que el nombreEnfermedades se está asignando correctamente
                    formVerTratamientos.nombreEnfermedades = nombreSeleccionado;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                // Verificar si FormDetalleEnfermedades fue abierto desde FormHistorialMedico
                if (formHistorialMedico != null)
                {
                    string codigoSeleccionado = row.Cells[1].Value.ToString();
                    string codigoEnfermedad = row.Cells[0].Value.ToString();

                    // Pasar el código seleccionado a FormHistorialMedico
                    formHistorialMedico.NombreEnfermedad = codigoSeleccionado;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                if (formEditarHistorialMedico != null)
                {
                    string codigoSeleccionado = row.Cells[1].Value.ToString();
                    string codigoEnfermedad = row.Cells[0].Value.ToString();

                    // Pasar el código seleccionado a FormHistorialMedico
                    formEditarHistorialMedico.NombreEnfermedad = codigoSeleccionado;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                // Verifica si se ha hecho clic en la última celda de la fila
                if (e.ColumnIndex == dataGridDetalleEnfermedad.Columns.Count - 1)  // Última celda de la fila
                {
                    // Obtén el valor de la celda
                    var cellValue = row.Cells[e.ColumnIndex].Value;

                    // Abre el formulario FormDetalles
                    FormObservaciones formDetalles = new FormObservaciones
                    {
                        SomeProperty = cellValue?.ToString(),
                        LabelText = "Síntomas"
                    };

                    formDetalles.ShowDialog(); // Mostrar como cuadro de diálogo modal
                }

                // Verifica si se ha hecho clic en la penúltima celda de la fila
                if (e.ColumnIndex == dataGridDetalleEnfermedad.Columns.Count - 2)  // Penúltima celda de la fila
                {
                    // Obtén el valor de la celda
                    var cellValue = row.Cells[e.ColumnIndex].Value;

                    // Abre el formulario FormDetalles
                    FormObservaciones formDetalles = new FormObservaciones
                    {
                        SomeProperty = cellValue?.ToString(),
                        LabelText = "Descripción"
                    };

                    formDetalles.ShowDialog();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.eliminarEnfermedad(dataGridDetalleEnfermedad);
            objetoEnfermedad.mostrarEnfermedades(dataGridDetalleEnfermedad);
        }
        public void ConfigurarAutocompletado(TextBox textBox)
        {
            try
            {
                Mysql.Cenfermedad objetoEnfermedades = new Mysql.Cenfermedad();

                // Obtener los nombres de enfermedades
                List<string> nombrespacientes = objetoEnfermedades.ObtenerNombreEnfermedades();

                // Crear la colección para el autocompletado
                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                autoCompleteCollection.AddRange(nombrespacientes.ToArray());

                // Configurar el autocompletado en el TextBox
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteCustomSource = autoCompleteCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar autocompletado: " + ex.Message);
            }
        }
    }
}