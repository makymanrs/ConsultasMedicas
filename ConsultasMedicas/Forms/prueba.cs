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
    public partial class prueba : Form
    {
        public prueba()
        {
            InitializeComponent();
        }
        public string SomeProperty
        {
            set { richTextBox1.Text = value; } // Asume que el RichTextBox se llama richTextBoxInfo
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
