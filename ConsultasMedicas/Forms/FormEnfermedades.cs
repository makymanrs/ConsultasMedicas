using ConsultasMedicas.Mysql;
using MySql.Data.MySqlClient;
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
    public partial class FormEnfermedades : Form
    {
        public FormEnfermedades()
        {
            InitializeComponent();
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.mostrarTratamientosPorEnfermedad(dataGridEnfermedad);
            foreach (DataGridViewColumn column in dataGridEnfermedad.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(0); // Ajusta según lo necesario
            }
            dataGridEnfermedad.RowTemplate.Height = 60;
            dataGridEnfermedad.ReadOnly = true;
            dataGridEnfermedad.BorderStyle = BorderStyle.FixedSingle;
            dataGridEnfermedad.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ActualizarConteoRegistros();
            ConfigurarAutocompletado(textBox1);

        }
        private void ActualizarConteoRegistros()
        {
            int totalRegistros = dataGridEnfermedad.RowCount;
            if (dataGridEnfermedad.AllowUserToAddRows)
            {
                totalRegistros--;
            }

            label4.Text = "Total de registros: " + totalRegistros;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.buscarTratamientosPorNombreEnfermedad(dataGridEnfermedad, textBox1);
            objetoEnfermedad.mostrarDetallesEnfermedad(textBox1, richTextBox1, richTextBox2);
            ActualizarConteoRegistros();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FormInsertarEnfermedad formInsertar = new FormInsertarEnfermedad();
            // formInsertar.RegistroGuardado += FormInsertar_RegistroGuardado; // Suscríbete al evento
            formInsertar.ShowDialog(); // Mostrar el formulario de inserción
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mysql.Cenfermedad objetoEnfermedad = new Mysql.Cenfermedad();
            objetoEnfermedad.mostrarTratamientosPorEnfermedad(dataGridEnfermedad);
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            textBox1.Text = "";
            ActualizarConteoRegistros();
        }

      
  
        private void button6_Click(object sender, EventArgs e)
        {
            FormDetalleEnfermedades mostrarForm = new FormDetalleEnfermedades();

            // Mostrar el formulario de manera modal
            mostrarForm.ShowDialog();
        }

        private void dataGridEnfermedad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no se haya hecho clic en el encabezado de columna o fuera del rango de celdas
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Verifica si se ha hecho clic en la última celda de la fila
            if (e.ColumnIndex == dataGridEnfermedad.Columns.Count - 1)  // Última celda de la fila
            {
                // Obtén el valor de la celda
                var cellValue = dataGridEnfermedad.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Abre el formulario FormDetalles
                FormObservaciones formDetalles = new FormObservaciones();

                // Opcionalmente, puedes pasarle información al nuevo formulario
                formDetalles.SomeProperty = cellValue?.ToString();

                formDetalles.ShowDialog(); // Mostrar como cuadro de diálogo modal
            }
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
