using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Para conectar con XAMPP
using Microsoft.Reporting.WinForms; // Para el ReportViewer

namespace RDLC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Se llama al método para cargar los datos y el informe
            CargarInforme();
        }

        private void CargarInforme()
        {
            // 1. Configuración de la conexión a XAMPP
            string connectionString = "Server=localhost;Database=TiendaDB;Uid=root;Pwd=;";
            // Se añade 'Id' a la consulta para que no aparezca vacío en el informe
            string query = "SELECT Id, Nombre, Precio, Categoria, Stock FROM Productos";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // 2. Configurar el ReportViewer
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    // Ruta del archivo RDLC (asegúrate de que esté en la carpeta bin/Debug)
                    reportViewer1.LocalReport.ReportPath = "InformeProductos.rdlc";

                    // --- LÍNEAS AÑADIDAS PARA EL FILTRO (Punto 4 de la práctica) ---
                    // Creamos el parámetro que espera el informe
                    // Si tienes un TextBox o ComboBox, puedes sustituir "Periféricos" por su .Text
                    ReportParameter[] parametros = new ReportParameter[1];
                    parametros[0] = new ReportParameter("pCategoria", "Periféricos");

                    // Pasamos el parámetro al informe antes de refrescarlo
                    reportViewer1.LocalReport.SetParameters(parametros);
                    // -------------------------------------------------------------

                    // 3. Refrescar para mostrar datos con el filtro aplicado
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
        }
    }
}