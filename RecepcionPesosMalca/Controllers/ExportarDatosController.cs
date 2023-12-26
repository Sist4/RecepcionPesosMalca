using RecepcionPesosMalca.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecepcionPesosMalca.Controllers
{
    public class ExportarDatosController
    {
        public List<RECEPCION> ExtraerDatosRecepciones(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var contexto = new MalcaDB())
            {
                // Consulta para filtrar por rango de fechas
                return  contexto.RECEPCIONES
                    .Where(r => DbFunctions.TruncateTime(r.FechaHora) >= fechaInicio && 
                    DbFunctions.TruncateTime(r.FechaHora) <= fechaFin)
                    .ToList();

               
            }

        }

        public void GuardarExcelRecepciones(string filePath, DateTime fechaInicio, DateTime fechaFin)
        {
            //string fecha = DateTime.Now.ToString("dd-MM-yyyy");
            SLDocument sl = new SLDocument();
            var datos = ExtraerDatosRecepciones(fechaInicio, fechaFin);
            int iC = 1;

            string[] encabezados = { "Báscula", "Peso", "Unidad", "FechaHora" };

            foreach (var encabezado in encabezados)
            {
                sl.SetCellValue(1, iC, encabezado);
                iC++;
            }



            int iR = 2;
            foreach (var dato in datos)
            {
                sl.SetCellValue(iR, 1, dato.Bascula);
                sl.SetCellValue(iR, 2, dato.Peso.ToString());
                sl.SetCellValue(iR, 3, dato.Unidad);
                sl.SetCellValue(iR, 4, dato.FechaHora.ToString());
   
                iR++;
            }
            sl.SaveAs(filePath);
            //MessageBox.Show("Se exportó el archivo excel");
        }
    }
}
