using RecepcionPesosMalca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecepcionPesosMalca.Controllers
{
    public class RecepcionController
    {
        DataGridView dtgDatos;
        DataGridView dtgIps;
        static List<Thread> threads;
        List<ServerInfo> servers;
        public RecepcionController(DataGridView dtgDatos, DataGridView dtgIps)
        {
            this.dtgDatos = dtgDatos;
            this.dtgIps = dtgIps;
            servers = new List<ServerInfo>();
            threads = new List<Thread>();
        }
        public void AgregarEquipo(string ip, string puerto)
        {
            if (!string.IsNullOrEmpty(ip) || !string.IsNullOrEmpty(puerto))
            {
                // Agregar los datos al DataGridView
                dtgIps.Rows.Add(ip, puerto);

                // Limpiar el TextBox después de agregar los datos
            }
            else
            {
                MessageBox.Show("Ingrese datos antes de agregar.");
            }
        }
        public void IniciarCapturaPesos()
        {
            // Listado de servidores (cambia con tus propias direcciones IP y puertos)
            CargarServerInfo();

            // Crear hilos para la recepción de respuestas
            foreach (var server in servers)
            {
                Thread receiveThread = new Thread(() => ReceiveResponses(server));
                receiveThread.Start();
                threads.Add(receiveThread);
            }
        }
        public void DetenerCapturaPesos()
        {
            foreach (var thread in threads)
            {
                thread.Abort();
            }
        }

        public static void DetenerCapturaPesosFormulario()
        {
            if (threads != null )
            {
                foreach (var thread in threads)
                {
                    thread.Abort();
                }

            }
            
        }


        public void ReceiveResponses(ServerInfo server)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient(server.Ip, server.Puerto))
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    //Console.WriteLine($"Conectado a {server.Ip}:{server.Port}");

                    while (true)
                    {
                        //Thread.Sleep(5000);
                        // Leer datos del servidor
                        byte[] buffer = new byte[1024];
                        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            string[] datos = receivedData.Split('\n');
                            string dato_peso = datos[0].Trim().Replace("\r", "");
                            string[] peso_separado = dato_peso.Split(' ');
                            string dato_nombre = datos[1].Trim().Replace("\r", "");
                            var recep = new RECEPCION
                            {
                                Bascula = dato_nombre,
                                Peso = float.Parse(peso_separado[0]),
                                Unidad = peso_separado[1],
                                FechaHora = DateTime.Now
                            };
                            MostrarPesosRecibidos(recep);
                            GuardarPesosRecibidos(recep);
                        }
                        else
                        {
                            //Console.WriteLine($"El servidor {server.Ip}:{server.Port} ha cerrado la conexión.");
                            //break;
                            //int bytesRead2 = networkStream.Read(buffer, 0, buffer.Length);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con {server.Ip}:{server.Puerto}: {ex.Message}");
            }
        }

        public void CargarServerInfo()
        {
            // Limpiar la lista antes de volver a llenarla
            servers.Clear();

            // Recorrer todas las filas del DataGridView
            foreach (DataGridViewRow row in dtgIps.Rows)
            {
                // Verificar si la fila no es nueva ni es la fila de encabezado
                if (!row.IsNewRow)
                {
                    // Obtener los valores de las celdas de IP y Puerto
                    string ip = Convert.ToString(row.Cells["Ip"].Value);
                    int puerto = Convert.ToInt32(row.Cells["Puerto"].Value);

                    // Crear una nueva instancia de ServerInfo y agregarla a la lista
                    ServerInfo server = new ServerInfo { Ip = ip, Puerto = puerto };
                    servers.Add(server);
                }
            }
        }

        public void MostrarPesosRecibidos(RECEPCION recepcion)
        {
            dtgDatos.BeginInvoke((MethodInvoker)delegate
            {
                dtgDatos.Rows.Add(recepcion.Bascula, recepcion.Peso, recepcion.Unidad, 
                    recepcion.FechaHora.ToString());
            });
        }
        public void GuardarPesosRecibidos(RECEPCION recepcion)
        {
            using(var context=new MalcaDB())
            {
                context.RECEPCIONES.Add(recepcion);
                context.SaveChanges();
            }
        }
    }
}
