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

        private void button4_Click(object sender, EventArgs e)
        {
            FormEditarEnfermedad formEditar = new FormEditarEnfermedad();
            formEditar.ShowDialog();
        }
  
        private void button6_Click(object sender, EventArgs e)
        {
            FormDetalleEnfermedades Formdetalle = new FormDetalleEnfermedades();
            Formdetalle.ShowDialog();   
        }

      
    }
}
