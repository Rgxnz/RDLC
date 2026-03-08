using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            CargarInforme();
            this.reportViewer1.RefreshReport();
        }

        private void CargarInforme()
        {
            // 1. Configuración de la conexión a XAMPP
            // Cambia 'TiendaDB' por el nombre de tu base de datos
            string connectionString = "Server=localhost;Database=TiendaDB;Uid=root;Pwd=;";
            string query = "SELECT Nombre, Precio, Categoria, Stock FROM Productos";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt); // El DataSet actúa como puente

                    // 2. Configurar el ReportViewer
                    reportViewer1.LocalReport.DataSources.Clear();

                    // El nombre "DataSet1" debe ser el que pusiste al agregar el Dataset al informe RDLC
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt); 
                    
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    reportViewer1.LocalReport.ReportPath = "InformeProductos.rdlc"; // Nombre de tu archivo de informe [cite: 31]

                    // 3. Refrescar para mostrar datos
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
