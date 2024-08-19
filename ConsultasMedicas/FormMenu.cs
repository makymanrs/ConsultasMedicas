using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas
{
    public partial class FormMenu : MaterialForm
    {
        // importante para saber el nombre de usuario que se loguea
        private string usuario;

        public FormMenu(string usuario)
        {
            // parametros para el diseño, encabezado, redondeado, palabras reservadas para la libreria material skin
            // en caso de error en el Form Menu entrar al nuget y buscar 
            InitializeComponent();
            this.usuario = usuario;
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700,
                Primary.Grey900,
                Primary.BlueGrey500,
                Accent.Red200,
                TextShade.WHITE);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada
            label2.Font = new Font(Font.FontFamily, 12, FontStyle.Bold);
            label3.Font = new Font(Font.FontFamily, 12, FontStyle.Bold);
            label4.Font = new Font(Font.FontFamily, 12, FontStyle.Bold);

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
        // el label que nos permite ver nuestro nombre de usuario
        private void FormMenu_Load(object sender, EventArgs e)
        {
            label1.Text = $"Bienvenido, {usuario}";
            CenterPanel2(panel3);
        }
        // resuelve el problema del form login que seguia en ejecucicion aqui lo cierra completamente al detectar que el form se cierre
        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        // esta parte es para el maximizado y minimizado
        private void FormMenu_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Region = null; // Restablece la región a null al maximizar
                //PlacePanelTopLeft(panel3);
                CenterPanel(panel3);

            }
            else if (WindowState == FormWindowState.Normal)
            {
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // Aplica la región redondeada al volver estado normal
                CenterPanel2(panel3);
            }
        }
        private void CenterPanel(Panel panel)
        {
            // Calcula las coordenadas X e Y para centrar el panel en el formulario
            int x = (this.ClientSize.Width - panel.Width) / 2;
            int y = (this.ClientSize.Height - panel.Height) / 2 - 100;
            // Establece la ubicación del panel
            panel.Location = new Point(x, y);
        }
        private void CenterPanel2(Panel panel)
        {
            // Calcula las coordenadas X e Y para centrar el panel en el formulario
            int x = (this.ClientSize.Width - panel.Width) / 2-40;
            int y = (this.ClientSize.Height - panel.Height) / 2 - 50;
            // Establece la ubicación del panel
            panel.Location = new Point(x, y);
        }
        /*
        private void PlacePanelTopLeft(Panel panel)
        {
            // Establece la ubicación del panel en el punto (0, 0)
            panel.Location = new Point(0, 0);
        }
        */
        public void ChangeTab(int tabIndex)
        {
            if (tabIndex >= 0 && tabIndex < materialTabControl1.TabPages.Count)
            {
                materialTabControl1.SelectedIndex = tabIndex;
            }
        }

        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialTabControl1.SelectedTab == tabPage2)
            {
                // Crear instancia del formulario que deseas mostrar
                Forms.FormPaciente formulario2 = new Forms.FormPaciente();
                formulario2.TopLevel = false;
                formulario2.FormBorderStyle = FormBorderStyle.None;
                formulario2.Dock = DockStyle.Fill;
                tabPage2.Controls.Clear(); // Limpiar cualquier control existente
                tabPage2.Controls.Add(formulario2);
                formulario2.Show();
            }
            if (materialTabControl1.SelectedTab == tabPage3)
            {
                // Crear instancia del formulario que deseas mostrar
                Forms.FormEnfermedades formulario3 = new Forms.FormEnfermedades();
                formulario3.TopLevel = false;
                formulario3.FormBorderStyle = FormBorderStyle.None;
                formulario3.Dock = DockStyle.Fill;
                tabPage3.Controls.Clear(); // Limpiar cualquier control existente
                tabPage3.Controls.Add(formulario3);
                formulario3.Show();
            }
            if (materialTabControl1.SelectedTab == tabPage4)
            {
                // Crear instancia del formulario que deseas mostrar
                Forms.FormTratamiento formulario4 = new Forms.FormTratamiento();
                formulario4.TopLevel = false;
                formulario4.FormBorderStyle = FormBorderStyle.None;
                formulario4.Dock = DockStyle.Fill;
                tabPage4.Controls.Clear(); // Limpiar cualquier control existente
                tabPage4.Controls.Add(formulario4);
                formulario4.Show();
            }
        }
    }
}
