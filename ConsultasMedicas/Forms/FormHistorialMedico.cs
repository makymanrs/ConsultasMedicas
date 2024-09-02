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

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
