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
    public partial class FormTratamiento : Form
    {
        public FormTratamiento()
        {
            InitializeComponent();
            
            ConfigurarDataGridView();
            
        }
        // aqui se llena el campo al abrir el form de DetalleEnfermedades
        public string CodigoEnfermedad
        {
            set { textBox1.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormDetalleEnfermedades mostrarForm = new FormDetalleEnfermedades(this);
            mostrarForm.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Mysql.Ctratamiento objetoTratamiento = new Mysql.Ctratamiento();
            objetoTratamiento.BuscarEnfermedadPorCodigo(textBox1, textBox2);
        }
        //toda la informacion del datagridview
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

            // Agregar columnas al DataGridView manualmente
          //  dataGridTratamiento.Columns.Add("idTtratamiento", "Id Tratamiento");
            dataGridTratamiento.Columns.Add("medicamento", "Medicamento");
            dataGridTratamiento.Columns.Add("dosis", "Dosis");
            dataGridTratamiento.Columns.Add("duracion", "Duración");
            dataGridTratamiento.Columns.Add("observaciones", "Observaciones");
            
        }
        private string ConvertirViñetasARichTextBox(RichTextBox richTextBox)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in richTextBox.Lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Añadir un símbolo para simular la viñeta, por ejemplo, un guion
                    sb.AppendLine("• " + line);
                }
            }
            return sb.ToString().Trim();
        }

        // para agregar los campos al datagridview
        private void button2_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los controles
            string medicamento = textBox3.Text.Trim();
            string dosis = textBox4.Text.Trim();
            string duracion = textBox5.Text.Trim();

            // Convertir las viñetas del RichTextBox en un formato de lista de texto
            string observaciones = ConvertirViñetasARichTextBox(richTextBox1);

            // Verificar si alguno de los campos está vacío
            if (string.IsNullOrWhiteSpace(medicamento) ||
                string.IsNullOrWhiteSpace(dosis) ||
                string.IsNullOrWhiteSpace(duracion) ||
                string.IsNullOrWhiteSpace(observaciones))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de agregar una fila.");
                return; // No agrega la fila si algún campo está vacío
            }

            // Agregar una nueva fila al DataGridView
            dataGridTratamiento.Rows.Add(medicamento, dosis, duracion, observaciones);

            // Limpiar los campos después de agregar la fila
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            richTextBox1.Text = "";
        }
        // Quita los campos que estan en el datagridview
        private void button3_Click(object sender, EventArgs e)
        {
            // Verificar si hay filas en el DataGridView
            if (dataGridTratamiento.Rows.Count == 0)
            {
                MessageBox.Show("No hay información en el DataGridView para eliminar.");
                return; // Salir del método si no hay filas
            }

            // Verificar si hay una fila seleccionada
            if (dataGridTratamiento.SelectedRows.Count > 0)
            {
                // Eliminar la fila seleccionada
                foreach (DataGridViewRow row in dataGridTratamiento.SelectedRows)
                {
                    // No eliminar la fila si es la fila de encabezado o si ya ha sido eliminada en un bucle anterior
                    if (!row.IsNewRow)
                    {
                        dataGridTratamiento.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mysql.Ctratamiento objetoTratamiento = new Mysql.Ctratamiento();

            // Crear una conexión a la base de datos
            MySqlConnection conexion = null;
            try
            {
                Conexion objetoConexion = new Conexion();
                conexion = objetoConexion.establecerConexion();

                // Obtener el ID de la enfermedad del TextBox
                string idEnfermedad = textBox1.Text.Trim();

                // Llamar al método para insertar los tratamientos
                objetoTratamiento.InsertarTratamiento(idEnfermedad, dataGridTratamiento, conexion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los tratamientos: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión si está abierta
                if (conexion != null && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            };
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBullet = true;
        }
    }
}
