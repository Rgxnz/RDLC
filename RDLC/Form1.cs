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
        // Cadena de conexión a XAMPP compartida
        private string connectionString = "Server=localhost;Database=TiendaDB;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Cargamos las categorías en el ComboBox al iniciar 
            LlenarCategorias();

            // 2. Cargamos el informe inicialmente con la primera categoría si existe
            if (cmbCategorias.Items.Count > 0)
            {
                cmbCategorias.SelectedIndex = 0;
                CargarInforme(cmbCategorias.Text);
            }
        }

         // Método para obtener las categorías únicas de la base de datos [cite: 187]
        private void LlenarCategorias()
        {
            string query = "SELECT DISTINCT Categoria FROM Productos";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbCategorias.DisplayMember = "Categoria";
                    cmbCategorias.ValueMember = "Categoria";
                    cmbCategorias.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }

        // Evento del botón para filtrar (Debes crear el botón btnFiltrar en el diseño) 
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            // 1. Validamos que haya una categoría seleccionada en el ComboBox
            if (cmbCategorias.SelectedValue != null)
            {
                // 2. Obtenemos el texto de la categoría elegida
                string categoriaSeleccionada = cmbCategorias.Text;

                // 3. Llamamos al método CargarInforme pasando la categoría como argumento
                // Esto sustituye el valor fijo de "Periféricos" por la elección del usuario 
                CargarInforme(categoriaSeleccionada);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una categoría para filtrar.");
            }
        }

        private void CargarInforme(string categoriaFiltro)
        {
            // IMPORTANTE: Incluimos todos los campos requeridos por la práctica 
            string query = "SELECT Id, Nombre, Precio, Categoria, Stock FROM Productos";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt); // El DataTable sirve de puente entre MySQL y el RDLC 

                    // 1. Limpiar y configurar el origen de datos del ReportViewer
                    reportViewer1.LocalReport.DataSources.Clear();

                    // El nombre "DataSet1" debe coincidir con el definido en el archivo .rdlc 
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    // Establecemos la ruta del archivo [cite: 30]
                    reportViewer1.LocalReport.ReportPath = "InformeProductos.rdlc";

                    // 2. Configuración del PARÁMETRO DINÁMICO (Punto 4 de la práctica)
                    // Usamos el valor pasado por argumento desde el ComboBox
                    ReportParameter[] parametros = new ReportParameter[1];
                    parametros[0] = new ReportParameter("pCategoria", categoriaFiltro);

                    // Pasamos el parámetro al motor de informes
                    reportViewer1.LocalReport.SetParameters(parametros);

                    // 3. Refrescar el visor para aplicar datos y filtros
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el informe: " + ex.Message);
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}