using RecepcionPesosMalca.Controllers;
using RecepcionPesosMalca.Models;
using RecepcionPesosMalca.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecepcionPesosMalca
{
    public partial class VPrincipal : Form
    {
        public VPrincipal()
        {
            InitializeComponent();
        }
        public void AbrirForm(Form form)
        {
            while (pnlPrincipal.Controls.Count > 0)
            {
                pnlPrincipal.Controls.RemoveAt(index: 0);
            }

            Form formHijo = form;
            form.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            pnlPrincipal.Controls.Add(formHijo);
            formHijo.Show();

        }

        private void btnRecepcion_Click(object sender, EventArgs e)
        {
            AbrirForm(new VRcecepciones());
        }

        private void VPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            RecepcionController.DetenerCapturaPesosFormulario();
            
        }

        private void btnExportarDatos_Click(object sender, EventArgs e)
        {
            AbrirForm(new VExportarDatos());
        }
    }
}
