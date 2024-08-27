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
    public partial class FormPaciente : Form
    {
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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridPaciente.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener los datos de la fila seleccionada
                int idPaciente = Convert.ToInt32(dataGridPaciente.SelectedRows[0].Cells["ID"].Value);
                string nombreCompleto = dataGridPaciente.SelectedRows[0].Cells["Nombre Completo"].Value.ToString();
                string identidad = dataGridPaciente.SelectedRows[0].Cells["Identidad"].Value.ToString();
                string fechaNacimiento = dataGridPaciente.SelectedRows[0].Cells["Fecha de Nacimiento"].Value.ToString();
                string edad = dataGridPaciente.SelectedRows[0].Cells["Edad"].Value.ToString();
                string direccion = dataGridPaciente.SelectedRows[0].Cells["Direccion"].Value.ToString();
                string telefono = dataGridPaciente.SelectedRows[0].Cells["Telefono"].Value.ToString();

                // Dividir nombre completo en nombre y apellido
                string[] nombrePartes = nombreCompleto.Split(' ', (char)2); // Divide en el primer espacio

                string nombre = nombrePartes.Length > 0 ? nombrePartes[0] : string.Empty;
                string apellido = nombrePartes.Length > 1 ? nombrePartes[1] : string.Empty;

                // Crear el formulario de edición con los datos obtenidos
                FormEditarPaciente formEditar = new FormEditarPaciente(idPaciente, nombre, apellido, identidad, fechaNacimiento, edad, direccion, telefono);
                formEditar.PacienteModificado += FormModificar_PacienteModificado; // Suscríbete al evento de modificación
                formEditar.ShowDialog();

                // Opcional: refrescar el DataGridView después de editar
                // button2_Click(sender, e); // Asumiendo que button2_Click refresca el DataGridView
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para editar.");
            }
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
    }
}
