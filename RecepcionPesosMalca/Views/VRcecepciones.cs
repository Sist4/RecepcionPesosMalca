using MaterialSkin;
using RecepcionPesosMalca.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecepcionPesosMalca.Views
{
    public partial class VRcecepciones : Form
    {
        RecepcionController controlador;
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        public VRcecepciones()
        {
            InitializeComponent();
            controlador=new RecepcionController(dtgDatos,dtgIps);

            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
                MaterialSkin.Primary.Indigo700,
                MaterialSkin.Primary.Indigo900,
                MaterialSkin.Primary.Indigo100,
                MaterialSkin.Accent.Blue700,
                MaterialSkin.TextShade.WHITE
                );
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            controlador.AgregarEquipo(txtIP.Text, txtPuerto.Text);
            LimpiarComponentes();
        }

        private void swtchCapturarPesos_CheckedChanged(object sender, EventArgs e)
        {
            if(swtchCapturarPesos.Checked)
            {
                controlador.IniciarCapturaPesos();
            }
            else
            {
                controlador.DetenerCapturaPesos();
            }
        }

        public void LimpiarComponentes()
        {
            txtIP.Text=string.Empty;
            txtPuerto.Text=string.Empty;
        }
    }
}
