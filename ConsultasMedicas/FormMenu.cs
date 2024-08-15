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
                Accent.LightBlue400,
                TextShade.WHITE);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada

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
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // Aplica la región redondeada al volver estado normal
            }
        }
    }
}
