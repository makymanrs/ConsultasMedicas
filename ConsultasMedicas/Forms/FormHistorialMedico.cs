using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas.Forms
{
    public partial class FormHistorialMedico : Form
    {
        public FormHistorialMedico()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            Mysql.Chistorialmedico objetoHistorialmedico = new Mysql.Chistorialmedico();
            objetoHistorialmedico.mostrarHistorialMedico(dataGridHistorialMedico);
            foreach (DataGridViewColumn column in dataGridHistorialMedico.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0); // Ajusta según lo necesario
            }
            dataGridHistorialMedico.RowTemplate.Height = 60;
            dataGridHistorialMedico.ReadOnly = true;

            dataGridHistorialMedico.BorderStyle = BorderStyle.FixedSingle;
            dataGridHistorialMedico.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ConfigurarAutocompletado(textBox3);
            ConfigurarAutocompletado2(textBox4);

        }
        public void lista()
        {
            comboBox1.Items.Add("Ninguno");
            comboBox1.Items.Add("Nombre Completo");
            comboBox1.Items.Add("Fecha de Consulta");
            comboBox1.SelectedIndex = 0;
        }
        public string NombreEnfermedad
        {
            set { textBox3.Text = value; } // Asumiendo que textBox2 es el TextBox para el nombre
        }
        public string CodigoPaciente
        {
            set { textBox1.Text = value; }
        }
        private string _tratamientos;
        public string trataminetos
        {
            get { return _tratamientos; }
            set { _tratamientos = value; richTextBox2.Text = _tratamientos; }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            FormPaciente mostrarForm = new FormPaciente(this);
            mostrarForm.ShowDialog();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            Mysql.Chistorialmedico objetoHistorial = new Mysql.Chistorialmedico();
            objetoHistorial.BuscarPacientePorCodigo(textBox1, textBox2);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FormDetalleEnfermedades mostrarForm = new FormDetalleEnfermedades(this);
            mostrarForm.ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string nombreEnfermedad = textBox3.Text;
            FormVerTratamientos mostrarForm = new FormVerTratamientos(this, nombreEnfermedad);
            mostrarForm.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Mysql.Chistorialmedico objetoHistorial = new Mysql.Chistorialmedico();
            objetoHistorial.guardarHistorialMedico(textBox1,textBox2 ,textBox3, dateTimePicker1, richTextBox1, richTextBox2);
        }

        private void FormHistorialMedico_Load(object sender, EventArgs e)
        {
            lista();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Mysql.Chistorialmedico objetoHistorial = new Mysql.Chistorialmedico();
            objetoHistorial.BuscarHistorialPorFiltros(dataGridHistorialMedico, textBox4,comboBox1,dateTimePicker2);
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            Mysql.Chistorialmedico objetoHistorialmedico = new Mysql.Chistorialmedico();
            objetoHistorialmedico.mostrarHistorialMedico(dataGridHistorialMedico);
        }
        public void ConfigurarAutocompletado(TextBox textBox)
        {
            try
            {
                // Crear una instancia de Cenfermedad
                Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();

                // Obtener los nombres de enfermedades
                List<string> nombresEnfermedades = objetoEnfermedad.ObtenerNombreEnfermedades();

                // Crear la colección para el autocompletado
                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                autoCompleteCollection.AddRange(nombresEnfermedades.ToArray());

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
        public void ConfigurarAutocompletado2(TextBox textBox)
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridHistorialMedico.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener los datos de la fila seleccionada
                int idHistorial = Convert.ToInt32(dataGridHistorialMedico.SelectedRows[0].Cells["ID Historial"].Value);
                string nombrePaciente = dataGridHistorialMedico.SelectedRows[0].Cells["Nombre del Paciente"].Value?.ToString() ?? string.Empty;
                DateTime fechaConsulta = Convert.ToDateTime(dataGridHistorialMedico.SelectedRows[0].Cells["Fecha de Consulta"].Value);
                string nombreEnfermedad = dataGridHistorialMedico.SelectedRows[0].Cells["Nombre de la Enfermedad"].Value?.ToString() ?? string.Empty;
                string diagnostico = dataGridHistorialMedico.SelectedRows[0].Cells["Diagnóstico"].Value?.ToString() ?? string.Empty;
                string tratamiento = dataGridHistorialMedico.SelectedRows[0].Cells["Tratamiento"].Value?.ToString() ?? string.Empty;

                // Crear el formulario de edición con los datos obtenidos
                FormEditarHistorialMedico formEditarHistorial = new FormEditarHistorialMedico(idHistorial, nombrePaciente, fechaConsulta, nombreEnfermedad, diagnostico, tratamiento);

                // Suscribirse al evento OnDataUpdated
                formEditarHistorial.OnDataUpdated += () =>
                {
                    // Actualiza los datos después de editar
                    LoadData();
                };

                formEditarHistorial.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para editar.");
            }

            // Configurar autocompletado después de editar
            ConfigurarAutocompletado(textBox1);
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Mysql.Chistorialmedico objetoHistorialmedico = new Mysql.Chistorialmedico();
            objetoHistorialmedico.eliminarHistorialMedico(dataGridHistorialMedico);
            objetoHistorialmedico.mostrarHistorialMedico(dataGridHistorialMedico);
           // ActualizarConteoRegistros();
        }
    }
}
