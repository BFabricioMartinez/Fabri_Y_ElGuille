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
using WindowsFormsApp2.Misc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp2
{
    public partial class Main : Form
    {
        private BusinessLogicLayer _businessLogicLayer;
        //private DataAccessLayer _dataAccessLayer;
        private DataAccessLayer dbHelper;
        private int pedidoActualId = 0;
        private int pedidoTempId = 0; // Para almacenar temporalmente el ID del pedido


        #region CONSTRUCTORS
        public Main()
        {
            InitializeComponent();
            dbHelper = new DataAccessLayer();
            _businessLogicLayer = new BusinessLogicLayer();
            CargarDatos();
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            btnCerrarPedido.Visible = false;
            GridPedidos.CellDoubleClick += GridPedidos_CellDoubleClick;
            GridPedidoItem.CellDoubleClick += GridPedidoItem_CellDoubleClick;
            GridPedidos.CellClick += GridPedidos_CellClick;



        }

        private void CargarDatos()
        {
            gridProductos.DataSource = dbHelper.ObtenerProductos();
            gridContacts.DataSource = dbHelper.ObtenerUsuarios();

            CargarComboBoxUsuarios();
            CargarComboBoxProductos();
            ConfigurarGrillaPedidos();
            ConfigurarGrillaPedidoItems();
            ActualizarGrillaPedidos();
            groupBox2.Enabled = false; // Deshabilita el groupBox2 inicialmente
            TxtPrecioPedido.ReadOnly = true; // Hace que el textbox del precio total sea de solo lectura
        }

        #endregion

        #region EVENTS_HANDLERS
        private void Main_Load(object sender, EventArgs e)
        {
            PopulateContacts();
        }

        #endregion

        #region METHODS
        private void OpenContactDetailDialog()
        {
            ContactDetails fichita = new ContactDetails();
            fichita.ShowDialog(this);
        }

        public void PopulateContacts(string searchTxt = null)
        {
            gridContacts.DataSource = _businessLogicLayer.GetAllContacts(searchTxt);
        }
        #endregion

        private void gridContacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = gridContacts.Rows[e.RowIndex].Cells[e.ColumnIndex];

            DataGridViewCell idToEdit = gridContacts.Rows[e.RowIndex].Cells[0];
            DataGridViewCell firstNameToEdit = gridContacts.Rows[e.RowIndex].Cells[1];
            DataGridViewCell lastNameToEdit = gridContacts.Rows[e.RowIndex].Cells[2];
            DataGridViewCell phoneToEdit = gridContacts.Rows[e.RowIndex].Cells[3];
            DataGridViewCell addressToEdit = gridContacts.Rows[e.RowIndex].Cells[4];


            switch (cell.Value.ToString())
            {
                case "Edit":


                    Contact contact = new Contact();

                    contact.Id = (int) idToEdit.Value;
                    contact.FirstName = (string) firstNameToEdit.Value;
                    contact.LastName = (string) lastNameToEdit.Value;
                    contact.Phone = (string) phoneToEdit.Value;
                    contact.Address = (string) addressToEdit.Value;

                    ContactDetails contactDetail = new ContactDetails();
                    contactDetail.LoadContactForm(contact);
                    contactDetail.ShowDialog(this);
                    break;
                case "Delete":

                    DialogResult res = MessageBox.Show("Seguro que queres eliminar el registro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.Yes)
                    {
                        DeleteContact((int)idToEdit.Value);
                        PopulateContacts();
                    }
                    
                    
                    break;
                default:
                    break;
            }

        }

        private void DeleteContact(int id)
        { 
            _businessLogicLayer.DeleteContact(id);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        // PRODUCTOS


        private void AgregarProducto()
        {
            try
            {
                // Validar que los campos estén llenos
                if (string.IsNullOrWhiteSpace(txtNombreProducto.Text) ||
                    string.IsNullOrWhiteSpace(txtCostoP.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioP.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios.");
                    return;
                }

                string nombreProducto = txtNombreProducto.Text;
                decimal costo;
                decimal precio;

                if (!decimal.TryParse(txtCostoP.Text, out costo) || !decimal.TryParse(txtPrecioP.Text, out precio))
                {
                    MessageBox.Show("Los campos de costo y precio deben ser valores numéricos.");
                    return;
                }

                // Llamada al método de dbHelper
                dbHelper.AgregarProducto(nombreProducto, costo, precio);

                // Limpiar campos y recargar la lista de productos
                LimpiarCampos();
                MessageBox.Show("Producto agregado correctamente.");
                gridProductos.DataSource = dbHelper.ObtenerProductos();
                gridProductos.AutoResizeColumns();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message + "\nNúmero de error: " + ex.Number);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message);
            }
        }

        // Boton AGREGAR en PRODUCTOS
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            AgregarProducto();
        }



        private void LimpiarCampos()
        {
            txtNombreProducto.Clear();
            txtCostoP.Clear();
            txtPrecioP.Clear();
        }

        private void txtNombreProducto_TextChanged(object sender, EventArgs e)
        {

        }

        // Boton Borrar PRODUCTOS
        private void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridProductos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un producto para borrar.");
                    return;
                }
                DataGridViewRow filaSeleccionada = gridProductos.SelectedRows[0];
                int id = Convert.ToInt32(filaSeleccionada.Cells["ID"].Value);

                DialogResult result = MessageBox.Show("¿Está seguro de que desea borrar este producto?", "Confirmar borrado", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dbHelper.BorrarProducto(id);

                    gridProductos.DataSource = dbHelper.ObtenerProductos();
                    LimpiarCampos();
                    MessageBox.Show("Producto borrado correctamente.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message + "\nNúmero de error: " + ex.Number);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al borrar producto: " + ex.Message);
            }
        }

        private void griProductos_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void gridProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabProductos_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabUsers;
        }

        private void iconButton2_Click(object sender, EventArgs e) //BOTON PRODUCTOS
        {
            tabControl1.SelectedTab = tabProductos;
        }

        private void iconButton3_Click(object sender, EventArgs e)//Boton INGRESAR PEDIDOS
        {
            tabControl1.SelectedTab = tabPedidos;
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            PopulateContacts(txtSearch.Text);
            txtSearch.Text = null;
        }

        private void iconButton5_Click(object sender, EventArgs e) //Button ADD
        {
            OpenContactDetailDialog();
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/guillelonder");
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {

        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/fabrii.martineez");
        }
        //PEDIDOS
        private void BoxPedidos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TxtPrecioPedido_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnAgregarPedido_Click(object sender, EventArgs e)
        {
            if (BoxPedidos.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un usuario");
                return;
            }
            ActualizarPrecioTotal();
            int firstName = Convert.ToInt32(BoxPedidos.SelectedValue);

            if (!decimal.TryParse(TxtPrecioPedido.Text, out var precioTotal))
            {
                MessageBox.Show("Error al convertir el precio total");
                return;
            }

            pedidoTempId = -1;
            pedidoActualId = 0;
            groupBox2.Enabled = true;
            BtnAgregarPedido.Visible = false;
            btnCerrarPedido.Visible = true;
        }


        private void btnCerrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (pedidoActualId <= 0)
                {
                    MessageBox.Show("No hay un pedido activo para cerrar.");
                    return;
                }

                if (GridPedidoItem.Rows.Count == 0)
                {
                    MessageBox.Show("No se puede cerrar un pedido sin items.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "¿Está seguro que desea guardar los cambios en este pedido?",
                    "Confirmar cambios de pedido",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {

                    groupBox2.Enabled = false;
                    BoxProductosP.SelectedIndex = -1;
                    BoxCant.Clear();
                    BoxPrecItem.Clear();

                    pedidoTempId = 0;
                    pedidoActualId = 0;

                    ActualizarGrillaPedidos();
                    MessageBox.Show("Pedido actualizado exitosamente.");

                    BtnAgregarPedido.Visible = true;
                    btnCerrarPedido.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar el pedido: " + ex.Message);
            }
        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (BoxProductosP.SelectedValue == null || !int.TryParse(BoxCant.Text, out var cantidad))
                {
                    MessageBox.Show("Debe seleccionar un producto y especificar la cantidad");
                    return;
                }

                int FirstName = Convert.ToInt32(BoxPedidos.SelectedValue);
                decimal precio = Convert.ToDecimal(BoxPrecItem.Text);
                int idProducto = Convert.ToInt32(BoxProductosP.SelectedValue);

                if (pedidoTempId == -1)
                {
                    decimal precioTotal = precio * cantidad;
                    pedidoActualId = dbHelper.AgregarPedido(FirstName, precioTotal);
                    if (pedidoActualId <= 0)
                    {
                        MessageBox.Show("Error al crear el pedido.");
                        return;
                    }
                    pedidoTempId = pedidoActualId;
                }

                dbHelper.AgregarPedidoItem(pedidoActualId, idProducto, cantidad, precio);
                ActualizarGrillaPedidoItems(pedidoActualId);
                BoxProductosP.SelectedIndex = -1;
                BoxCant.Clear();
                BoxPrecItem.Clear();
                ActualizarPrecioTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar item: " + ex.Message);
            }
        }

        private void GridPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int idPedido = Convert.ToInt32(GridPedidos.Rows[e.RowIndex].Cells["id"].Value);
                    ActualizarGrillaPedidoItems(idPedido);
                    pedidoActualId = idPedido;
                    groupBox2.Enabled = true;
                    BtnAgregarPedido.Visible = false;
                    btnCerrarPedido.Visible = true;

                    var pedido = dbHelper.ObtenerPedidoPorId(idPedido);
                    if (pedido != null)
                    {
                        BoxPedidos.SelectedValue = pedido.IdCliente;
                        TxtPrecioPedido.Text = pedido.total.ToString("N2");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar pedido: " + ex.Message);
            }
        }

        private void BoxProductosP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BoxProductosP.SelectedItem is DataRowView drv)
            {
                BoxPrecItem.Text = drv["PrecioProd"] != DBNull.Value ? drv["PrecioProd"].ToString() : "0";
            }
            else
            {
                BoxPrecItem.Text = "0";
            }
        }

        private void GridPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar este registro?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(GridPedidos.Rows[e.RowIndex].Cells["ID"].Value);
                        dbHelper.EliminarPedido(id);
                        GridPedidos.Rows.RemoveAt(e.RowIndex);

                        MessageBox.Show("Registro eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GridPedidoItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar este ítem de pedido?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int idPedidoItem = Convert.ToInt32(GridPedidoItem.Rows[e.RowIndex].Cells["ID"].Value);
                        dbHelper.EliminarPedidoItem(idPedidoItem);
                        GridPedidoItem.Rows.RemoveAt(e.RowIndex);

                        MessageBox.Show("Ítem de pedido eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el ítem de pedido: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void CargarComboBoxUsuarios()
        {
            try
            {
                var FirstName = dbHelper.ObtenerUsuariosPedidos();
                BoxPedidos.DataSource = FirstName;
                BoxPedidos.DisplayMember = "FirstName";
                BoxPedidos.ValueMember = "id";
                BoxPedidos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void CargarComboBoxProductos()
        {
            try
            {
                var productos = dbHelper.ObtenerProductos();
                BoxProductosP.DataSource = productos;
                BoxProductosP.DisplayMember = "UsuariosProd";
                BoxProductosP.ValueMember = "id";
                BoxProductosP.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void ConfigurarGrillaPedidos()
        {
            try
            {
                if (GridPedidos == null)
                {
                    GridPedidos = new DataGridView
                    {
                        Name = "GridPedidos",
                        Dock = DockStyle.Bottom,
                        Height = groupBox1.Height - 200,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        AllowUserToAddRows = false,
                        ReadOnly = true
                    };
                    groupBox1.Controls.Add(GridPedidos);
                }

                GridPedidos.DataSource = dbHelper.ObtenerPedidos();
                if (GridPedidos.Columns["id"] != null) GridPedidos.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar la grilla de pedidos: " + ex.Message);
            }
        }

        private void ConfigurarGrillaPedidoItems()
        {
            try
            {
                if (GridPedidoItem == null)
                {
                    GridPedidoItem = new DataGridView
                    {
                        Name = "gridItems",
                        Dock = DockStyle.Bottom,
                        Height = groupBox2.Height - 200,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        AllowUserToAddRows = false,
                        ReadOnly = true
                    };
                    groupBox2.Controls.Add(GridPedidoItem);
                    GridPedidoItem.Columns.Clear(); // Limpiar columnas anteriores
                    GridPedidoItem.AutoGenerateColumns = true; // Generar columnas automáticamente
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar la grilla de items: " + ex.Message);
            }
        }

        private void ActualizarGrillaPedidos()
        {
            try
            {
                GridPedidos.DataSource = dbHelper.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la grilla de pedidos: " + ex.Message);
            }
        }

        private void ActualizarGrillaPedidoItems(int idPedido)
        {
            try
            {
                DataTable dt = dbHelper.ObtenerPedidoItems(idPedido);
                GridPedidoItem.DataSource = dt;
                ActualizarPrecioTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los items del pedido: " + ex.Message);
            }
        }
        private void ActualizarPrecioTotal()
        {
            try
            {
                if (GridPedidoItem != null && GridPedidoItem.Rows.Count > 0)
                {
                    decimal total = 0;
                    foreach (DataGridViewRow row in GridPedidoItem.Rows)
                    {
                        if (row.Cells["subtotal"].Value != null && row.Cells["subtotal"].Value != DBNull.Value)
                        {
                            total += Convert.ToDecimal(row.Cells["subtotal"].Value);
                        }
                    }
                    TxtPrecioPedido.Text = total.ToString("N2");

                    // Actualizar el precio total en la base de datos
                    if (pedidoActualId > 0)
                    {
                        dbHelper.ActualizarPrecioTotalPedido(pedidoActualId, total);
                    }
                }
                else
                {
                    TxtPrecioPedido.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular el precio total: " + ex.Message);
                TxtPrecioPedido.Text = "0.00";
            }
        }
        private void BoxCant_TextChanged(object sender, EventArgs e)
        {

        }
        private void BoxPrecItem_TextChanged(object sender, EventArgs e)
        {

        }

        private void GridPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GridPedidoItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}




