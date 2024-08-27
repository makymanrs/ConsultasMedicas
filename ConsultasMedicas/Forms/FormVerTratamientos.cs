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
    public partial class FormVerTratamientos : Form
    {
        public FormVerTratamientos()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            
        }
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

        public string nombreEnfermedad
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
    }
}
