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
            // Solo llamamos al método, el RefreshReport se hace dentro de CargarInforme
            CargarInforme();
        }

        private void CargarInforme()
        {
            // 1. Configuración de la conexión a XAMPP (MySQL)
            string connectionString = "Server=localhost;Database=TiendaDB;Uid=root;Pwd=;";

            // IMPORTANTE: Incluimos 'Id' y 'Stock' para que no aparezcan vacíos en el informe [cite: 115, 203]
            string query = "SELECT Id, Nombre, Precio, Categoria, Stock FROM Productos";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt); // El DataTable sirve de puente entre MySQL y el RDLC [cite: 52, 161, 182]

                    // 2. Limpiar y configurar el origen de datos del ReportViewer
                    reportViewer1.LocalReport.DataSources.Clear();

                    // El nombre "DataSet1" debe coincidir con el nombre definido en el archivo .rdlc [cite: 13]
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    // Establecemos la ruta del archivo (debe estar configurado como 'Copiar siempre' en propiedades) [cite: 30]
                    reportViewer1.LocalReport.ReportPath = "InformeProductos.rdlc";

                    // 3. Configuración del PARÁMETRO DE FILTRO (Punto 4 de la práctica) [cite: 118, 214]
                    // Aquí definimos qué categoría queremos filtrar (ejemplo: 'Periféricos')
                    ReportParameter[] parametros = new ReportParameter[1];
                    parametros[0] = new ReportParameter("pCategoria", "Periféricos");

                    // Pasamos el parámetro al motor de informes
                    reportViewer1.LocalReport.SetParameters(parametros);

                    // 4. Refrescar el visor para aplicar datos, formatos y filtros
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}