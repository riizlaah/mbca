using MBCA_Desktop.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop.UserControls
{
    public partial class CRUDExhibits : UserControl
    {
        public CRUDExhibits()
        {
            InitializeComponent();
            Helper.GenTableColumns(table, ["Name", "Artist", "Category", "Time Periods", "Tags"], ["name", "artist", "categoryName", "timePeriod", "tagsStr"]);
            var editCol = new DataGridViewButtonColumn
            {
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
            };
            var delCol = new DataGridViewButtonColumn
            {
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
            };
            table.Columns.Add(editCol);
            table.Columns.Add(delCol);
            RefreshData();
        }

        async private Task RefreshData()
        {
            var (success, res, msg) = await Helper.jsonReq<List<Exhibit>, object>("exhibits");
            if (!success || res.data == null) return;
            table.DataSource = res.data;
        }

        private void onTableCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var item = table.CurrentCell.OwningRow.DataBoundItem as Exhibit;
                if (item == null) return;
                var window = new AddEditExhibitForm(true, item);
                window.ShowDialog();
                RefreshData();
            }
            if (e.ColumnIndex == 6)
            {
                TryDelete();
            }
        }

        async private Task TryDelete()
        {
            var item = table.CurrentCell.OwningRow.DataBoundItem as Exhibit;
            if (item == null) return;
            if (MessageBox.Show($"Are you sure want to delete {item.name}?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No) return;
            var (success, res, msg) = await Helper.jsonReq<object, object>($"exhibits/{item.id}", "delete");

            if(!success)
            {
                MessageBox.Show(msg);
                return;
            }
            RefreshData();
            
        }

        private void onAddNew(object sender, EventArgs e)
        {
            var window = new AddEditExhibitForm(false);
            window.ShowDialog();
            RefreshData();
        }
    }
}
