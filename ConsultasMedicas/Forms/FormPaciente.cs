using Org.BouncyCastle.Pqc.Crypto.Lms;
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

    public partial class FormPaciente : Form
    {
        private FormHistorialMedico formhistorialmedico;
        public FormPaciente()
        {
            InitializeComponent();
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.mostrarPaciente(dataGridPaciente);
            foreach (DataGridViewColumn column in dataGridPaciente.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0); // Ajusta según lo necesario
            }
            dataGridPaciente.RowTemplate.Height = 60;
            dataGridPaciente.ReadOnly = true;
            ActualizarConteoRegistros();
            ConfigurarAutocompletado(textBox1);
            button6.Visible=false;
        }
        public FormPaciente(FormHistorialMedico formhistorialMedico = null)
        {
            InitializeComponent();
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.mostrarPaciente(dataGridPaciente);

            foreach (DataGridViewColumn column in dataGridPaciente.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0);
            }
            dataGridPaciente.RowTemplate.Height = 60;
            dataGridPaciente.ReadOnly = true;
            ActualizarConteoRegistros();
            ConfigurarAutocompletado(textBox1);

            if (formhistorialMedico != null)
            {
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                FormBorderStyle = FormBorderStyle.None;
                ControlBox = false;
                button6.Visible = true;
                this.StartPosition = FormStartPosition.CenterScreen;
                label4.Visible = false;
                label2.Visible = false;
                panel3.BackColor = Color.FromArgb(33, 33, 33);
                panel3.Height = 27;
                panel3.MouseDown += Panel3_MouseDown;
                this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                this.BackColor = Color.FromArgb(255, 255, 238);
                this.formhistorialmedico = formhistorialMedico;
            }
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

        private void Panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public void listas()
        {
            comboBox1.Items.Add("Ninguno");
            comboBox1.Items.Add("Nombre Completo");
            comboBox1.Items.Add("ID");
            comboBox1.SelectedIndex = 0;
        }
        private void FormPaciente_Load(object sender, EventArgs e)
        {
            listas();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FormInsertarPaciente formInsertar = new FormInsertarPaciente();
            formInsertar.RegistroGuardado += FormInsertar_RegistroGuardado; // Suscríbete al evento
            formInsertar.ShowDialog(); // Mostrar el formulario de inserción
            ConfigurarAutocompletado(textBox1);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridPaciente.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener los datos de la fila seleccionada
                int idPaciente = Convert.ToInt32(dataGridPaciente.SelectedRows[0].Cells["ID"].Value);
                string nombreCompleto = dataGridPaciente.SelectedRows[0].Cells["Nombre Completo"].Value?.ToString() ?? string.Empty;
                string identidad = dataGridPaciente.SelectedRows[0].Cells["Identidad"].Value?.ToString() ?? string.Empty;
                string fechaNacimiento = dataGridPaciente.SelectedRows[0].Cells["Fecha de Nacimiento"].Value?.ToString() ?? string.Empty;
                string edad = dataGridPaciente.SelectedRows[0].Cells["Edad"].Value?.ToString() ?? string.Empty;
                string direccion = dataGridPaciente.SelectedRows[0].Cells["Dirección"].Value?.ToString() ?? string.Empty;
                string telefono = dataGridPaciente.SelectedRows[0].Cells["Teléfono"].Value?.ToString() ?? string.Empty;

                // Verificar si el nombre completo es nulo o vacío
                if (!string.IsNullOrWhiteSpace(nombreCompleto))
                {
                    // Asumimos que "Nombre Completo" es la concatenación de "pac_nombre" y "pac_apellido"
                    string[] partesNombreCompleto = nombreCompleto.Split(' ');

                    // Suponiendo que el primer y segundo nombres son los primeros dos elementos
                    string nombre = string.Join(" ", partesNombreCompleto.Take(2));

                    // Los demás elementos después de los dos primeros se consideran como apellidos
                    string apellido = string.Join(" ", partesNombreCompleto.Skip(2));

                    // Crear el formulario de edición con los datos obtenidos
                    FormEditarPaciente formEditar = new FormEditarPaciente(idPaciente, nombre, apellido, identidad, fechaNacimiento, edad, direccion, telefono);
                    formEditar.PacienteModificado += FormModificar_PacienteModificado; // Suscríbete al evento de modificación
                    formEditar.ShowDialog();
                }
                else
                {
                    // Si "Nombre Completo" está vacío, abrir el formulario con campos vacíos
                    FormEditarPaciente formEditar = new FormEditarPaciente(idPaciente, string.Empty, string.Empty, identidad, fechaNacimiento, edad, direccion, telefono);
                    formEditar.PacienteModificado += FormModificar_PacienteModificado; // Suscríbete al evento de modificación
                    formEditar.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para editar.");
            }
            ConfigurarAutocompletado(textBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.mostrarPaciente(dataGridPaciente);
            textBox1.Text = "";
            ActualizarConteoRegistros();
        }

        private void ActualizarConteoRegistros()
        {
            int totalRegistros = dataGridPaciente.RowCount;
            if (dataGridPaciente.AllowUserToAddRows)
            {
                totalRegistros--; 
            }

            label4.Text = "Total de registros: " + totalRegistros;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.eliminarPaciente(dataGridPaciente);
            objetoPaciente.mostrarPaciente(dataGridPaciente);
            ActualizarConteoRegistros();
        }

        private void FormInsertar_RegistroGuardado(object sender, EventArgs e)
        {
            // Actualizar la tabla cuando el registro es guardado
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.mostrarPaciente(dataGridPaciente);
            ActualizarConteoRegistros();
        }
        private void FormModificar_PacienteModificado(object sender, EventArgs e)
        {
            // Actualizar la tabla cuando el registro es modificado
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.mostrarPaciente(dataGridPaciente);
            ActualizarConteoRegistros();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mysql.Cpacientes objetoPaciente = new Mysql.Cpacientes();
            objetoPaciente.BuscarPacientesPorFiltros(dataGridPaciente, textBox1, comboBox1);
            ActualizarConteoRegistros();
        }

        private void dataGridPaciente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Mysql.Cpacientes objetoDetalle = new Mysql.Cpacientes();
            objetoDetalle.seleccionarPaciente(dataGridPaciente, textBox1);
        }

        private void dataGridPaciente_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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
        public void ConfigurarAutocompletado(TextBox textBox)
        {
            try
            {
                Mysql.Cpacientes objetoPacientes = new Mysql.Cpacientes();

                // Obtener los nombres de enfermedades
                List<string> nombrespacientes = objetoPacientes.ObtenerNombrePacientes();

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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            button6.BackColor = Color.Red;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void dataGridPaciente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridPaciente.Rows.Count)
            {
                if (formhistorialmedico != null)
                {
                    // Obtener el código seleccionado
                    string codigoSeleccionado = dataGridPaciente.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                    // Pasar el código seleccionado a FormHistorialMedico
                    formhistorialmedico.CodigoPaciente = codigoSeleccionado;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
