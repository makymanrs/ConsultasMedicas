using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultasMedicas.Forms
{
    public partial class FormObservaciones : Form
    {
        public FormObservaciones()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); // aqui se establece como sera la esquina redondeada
            AplicarJustificacion(richTextBox1);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Obtener el área del formulario
            Graphics g = e.Graphics;

            // Configura el color y el grosor del borde
            Pen borderPen = new Pen(Color.Black, 8); // Puedes ajustar el color y grosor aquí

            // Dibujar el borde alrededor del formulario
            g.DrawPath(borderPen, CreateRoundRectPath(0, 0, Width, Height, 20, 20)); // El grosor del borde es 2 y el radio de las esquinas es 20
        }

        private GraphicsPath CreateRoundRectPath(int x, int y, int width, int height, int radiusX, int radiusY)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radiusX, radiusY, 180, 90);
            path.AddArc(x + width - radiusX, y, radiusX, radiusY, 270, 90);
            path.AddArc(x + width - radiusX, y + height - radiusY, radiusX, radiusY, 0, 90);
            path.AddArc(x, y + height - radiusY, radiusX, radiusY, 90, 90);
            path.CloseFigure();
            return path;
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
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        public string SomeProperty
        {
            set { richTextBox1.Text = value; } // Asume que el RichTextBox se llama richTextBoxInfo
        }
        public string LabelText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(33, 33, 33);

        }
        private string JustificarTexto(string texto, int anchoLinea)
        {
            var lineas = texto.Split(new[] { '\n' }, StringSplitOptions.None);
            var textoJustificado = new StringBuilder();

            foreach (var linea in lineas)
            {
                var palabras = linea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (palabras.Length == 0)
                {
                    textoJustificado.AppendLine();
                    continue;
                }

                var anchoTexto = linea.Length;
                var espaciosNecesarios = anchoLinea - anchoTexto;

                if (espaciosNecesarios > 0)
                {
                    var espaciosEntrePalabras = palabras.Length - 1;
                    if (espaciosEntrePalabras > 0)
                    {
                        var espacioExtra = espaciosNecesarios % espaciosEntrePalabras;
                        var espacioBase = espaciosNecesarios / espaciosEntrePalabras;
                        for (var i = 0; i < palabras.Length - 1; i++)
                        {
                            textoJustificado.Append(palabras[i]);
                            textoJustificado.Append(new string(' ', espacioBase));
                            if (i < espacioExtra)
                            {
                                textoJustificado.Append(' ');
                            }
                        }
                        textoJustificado.Append(palabras[palabras.Length - 1]); // Acceder al último elemento con índice clásico
                    }
                    else
                    {
                        textoJustificado.Append(linea);
                    }
                }
                else
                {
                    textoJustificado.Append(linea);
                }

                textoJustificado.AppendLine();
            }

            return textoJustificado.ToString();
        }

        private void AplicarJustificacion(RichTextBox richTextBox)
        {
            var textoOriginal = richTextBox.Text;
            var anchoLinea = richTextBox.ClientSize.Width; // Ajustar según el tamaño del RichTextBox

            var textoJustificado = JustificarTexto(textoOriginal, anchoLinea);
            richTextBox.Text = textoJustificado;
        }
    }
}
