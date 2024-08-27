using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConsultasMedicas.Forms
{
    public partial class FormEditarTratamiento : Form
    {
        private int idTratamiento;
        private Mysql.Ctratamiento objetoTratamiento;
        private int id;
        private string nombre;
        private string descripcion;
        private string sintomas;

        public FormEditarTratamiento(int id, string nombre, string descripcion, string sintomas)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.sintomas = sintomas;
        }

        public FormEditarTratamiento(int id,string medicamento, string dosis, string duracion, string observaciones)
        {
            InitializeComponent();

            // Asignar los valores pasados a los TextBox y RichTextBox
            idTratamiento = id; // Guardar el id del tratamiento
            textBox3.Text = medicamento;
            textBox4.Text = dosis;
            textBox5.Text = duracion;
            richTextBox1.Text = observaciones;

            objetoTratamiento = new Mysql.Ctratamiento();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            objetoTratamiento.modificarTratamiento(idTratamiento, textBox3, textBox4, textBox5, richTextBox1);
            this.Close(); // Cerrar el formulario después de guardar los cambios
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBullet = true;
        }
    }
}
