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
    public partial class CRUDEvents : UserControl
    {
        public CRUDEvents()
        {
            InitializeComponent();
            Helper.GenTableColumns(table, ["Title", "Description", "Date & Time", "Location", "Initiator", "Price", "Category"], ["title", "description", "dateNTime", "location", "initiator", "price", "categoryName"]);
            Helper.GenTableColumns(table2, ["Name", "Artist", "Category", "Time Periods", "Tags"], ["name", "artist", "categoryName", "timePeriod", "tagsStr"]);
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
            var (success, res, msg) = await Helper.jsonReq<List<Event>, object>("events");
            if (!success || res.data == null) return;
            table.DataSource = res.data;
        }

        private void onEventCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            GetExhibits();
            if (e.ColumnIndex == 7)
            {
                var record = table.CurrentCell.OwningRow.DataBoundItem as Event;
                if (record == null) return;
                var window = new AddEditEventForm(true, record);
                window.ShowDialog();
                RefreshData();
            }
            if (e.ColumnIndex == 8)
            {
                TryDelete();
            }
        }
        async private Task TryDelete()
        {
            var item = table.CurrentCell.OwningRow.DataBoundItem as Event;
            if (item == null) return;
            if (MessageBox.Show($"Are you sure want to delete {item.title}?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No) return;
            var (success, res, msg) = await Helper.jsonReq<object, object>($"events/{item.id}", "delete");
            if (!success)
            {
                MessageBox.Show(msg);
                return;
            }
            RefreshData();

        }

        private void onAddEvent(object sender, EventArgs e)
        {
            var window = new AddEditEventForm(false);
            window.ShowDialog();
            RefreshData();
        }

        async private Task GetExhibits()
        {
            var record = table.CurrentCell.OwningRow.DataBoundItem as Event;
            if (record == null)
            {
                table2.DataSource = null;
                return;
            }
            var (success2, res2, msg2) = await Helper.jsonReq<List<Exhibit>, object>($"events/{record.id}/exhibits");
            if (res2.data == null || !success2)
            {
                MessageBox.Show("Failed to retrieve exhibits data");
                return;
            }
            table2.DataSource = res2.data;
        }
    }
}
