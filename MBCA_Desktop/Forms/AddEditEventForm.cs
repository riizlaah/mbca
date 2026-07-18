using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace MBCA_Desktop.Forms
{
    public partial class AddEditEventForm : Form
    {
        private bool editMode { get; set; } = false;
        private Event record { get; set; }
        private List<string> imageUrls { get; set; } = new List<string>();
        private List<string> unuploadedImageUrls { get; set; } = new List<string>();
        private int currentImgIdx { get; set; } = 0;
        private System.Windows.Forms.Timer timer;
        private Exhibit? selectedNewExhibit = null;
        private List<Exhibit> ownedExhibits = new List<Exhibit>();
        public AddEditEventForm(bool edit, Event? ev = null)
        {
            timer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
            editMode = edit;
            record = ev ?? new Event();
            InitializeComponent();
            exhibitsAutocomplete.Hide();
            timer.Tick += onSearchTimerTimeout;
            formTitle.Text = (edit ? "Edit" : "Add") + "Event";
            Text = (edit ? "Edit" : "Add") + "Event";
            categories.DisplayMember = "name";
            exhibitsAutocomplete.DisplayMember = "shortDetail";
            Helper.GenTableColumns(exhibitTable, ["Name", "Artist", "Category"], ["name", "artist", "categoryName"]);
            var rmCol = new DataGridViewButtonColumn
            {
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
            };
            exhibitTable.Columns.Add(rmCol);
            GetCategories();
        }

        async private Task GetCategories()
        {
            var (success, res, msg) = await Helper.jsonReq<List<EvCategory>, object>("events/categories");
            if (res.data == null || !success)
            {
                MessageBox.Show("Failed to retrieve categories data");
                return;
            }
            categories.DataSource = res.data;
            if (editMode && record != null)
            {
                title.Text = record.title;
                description.Text = record.description;
                location.Text = record.location;
                initiator.Text = record.initiator;
                price.Text = record.price.ToString();
                date.Value = record.date.ToDateTime(TimeOnly.MinValue);
                startTime.Value = record.date.ToDateTime(record.startTime);
                endTime.Value = record.date.ToDateTime(record.endTime);
                categories.SelectedIndex = res.data.FindIndex(p => p.id == record.category.id);
                imageUrls.AddRange(record.banners.Select(b => b.banner));
                image.Image = await Helper.GetImage(imageUrls[0]);
                var (success2, res2, msg2) = await Helper.jsonReq<List<Exhibit>, object>($"events/{record.id}/exhibits");
                if (res2.data == null || !success2)
                {
                    MessageBox.Show("Failed to retrieve exhibits data");
                    return;
                }
                ownedExhibits = res2.data;
                LocalExhibitRefresh();
            }
        }

        private void LocalExhibitRefresh()
        {
            exhibitTable.DataSource = new List<Exhibit>(ownedExhibits);
        }

        private void onCancel(object sender, EventArgs e)
        {
            Close();
        }

        private void onSubmit(object sender, EventArgs e)
        {
            TrySubmit();
        }

        async private Task TrySubmit()
        {
            if (!editMode && unuploadedImageUrls.Count == 0)
            {
                MessageBox.Show("Banner required");
                return;
            }
            var category = categories.SelectedItem as EvCategory;
            if (category == null)
            {
                MessageBox.Show("Category required");
                return;
            }
            if (!decimal.TryParse(price.Text, out decimal priceDec))
            {
                MessageBox.Show("Price not valid");
                return;
            }

            try
            {
                using var multipartForm = new MultipartFormDataContent();
                multipartForm.Add(new StringContent(title.Text), "title");
                multipartForm.Add(new StringContent(description.Text), "description");
                multipartForm.Add(new StringContent(location.Text), "location");
                multipartForm.Add(new StringContent(initiator.Text), "initiator");
                multipartForm.Add(new StringContent(priceDec.ToString()), "price");
                multipartForm.Add(new StringContent(category.id.ToString()), "categoryId");
                multipartForm.Add(new StringContent(date.Value.ToString("yyyy-MM-dd")), "date");
                multipartForm.Add(new StringContent(startTime.Value.ToString("hh:mm:ss")), "startTime");
                multipartForm.Add(new StringContent(endTime.Value.ToString("hh:mm:ss")), "endTime");
                if (unuploadedImageUrls.Count > 0)
                {
                    foreach (var url in unuploadedImageUrls)
                    {
                        var bytes = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        var content = new StreamContent(bytes);
                        var mimetype = (Path.GetExtension(url).Trim('.') == "png") ? "image/png" : "image/jpeg";
                        content.Headers.ContentType = new MediaTypeHeaderValue(mimetype);
                        multipartForm.Add(content, "banners", Path.GetFileName(url));
                    }
                }
                foreach(var ex in ownedExhibits)
                {
                    multipartForm.Add(new StringContent(ex.id.ToString()), "exhibits[]");
                }
                if (editMode)
                {
                    var (success, res, msg) = await Helper.multipartReq($"events/{record.id}", "put", multipartForm);
                    if (!success)
                    {
                        MessageBox.Show(msg, "Error");
                        return;
                    }
                }
                else
                {
                    var (success, res, msg) = await Helper.multipartReq("events", "post", multipartForm);
                    if (!success)
                    {
                        MessageBox.Show(msg, "Error");
                        return;
                    }
                }
                Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace, ex.Message);
            }
        }

        private void onImageClicked(object sender, EventArgs e)
        {
            chooseImage.ShowDialog();
            if (chooseImage.FileName == "") return;
            unuploadedImageUrls.Add(chooseImage.FileName);
            currentImgIdx = imageUrls.Count + unuploadedImageUrls.Count - 1;
            UpdateBanner(imageUrls.Count + unuploadedImageUrls.Count - 1);
        }


        private void onNextBanner(object sender, EventArgs e)
        {
            UpdateBanner(currentImgIdx + 1);
        }

        private void onPrevBanner(object sender, EventArgs e)
        {
            UpdateBanner(currentImgIdx - 1);
        }

        async private void UpdateBanner(int idx)
        {
            currentImgIdx = Math.Clamp(idx, 0, imageUrls.Count + unuploadedImageUrls.Count - 1);
            prevBtn.Enabled = currentImgIdx > 0;
            nextBtn.Enabled = currentImgIdx < imageUrls.Count + unuploadedImageUrls.Count - 1;
            if (currentImgIdx < imageUrls.Count)
            {
                image.Image = await Helper.GetImage(imageUrls[currentImgIdx]);
            }
            else
            {
                var bytes = File.ReadAllBytes(unuploadedImageUrls[currentImgIdx - imageUrls.Count]);
                using (var ms = new MemoryStream(bytes))
                {
                    image.Image = Image.FromStream(ms);
                }
            }
        }

        private void onExhibitNameChanged(object sender, EventArgs e)
        {
            if (selectedNewExhibit != null)
            {
                if (StringComparer.OrdinalIgnoreCase.Compare(exhibitName.Text, selectedNewExhibit.shortDetail) != 0)
                {
                    selectedNewExhibit = null;
                }
            }
            timer.Stop();
            timer.Start();
        }

        private async void onSearchTimerTimeout(object sender, EventArgs e)
        {
            timer.Stop();
            if (selectedNewExhibit != null) return;
            if (exhibitName.Text.Trim() == "")
            {
                exhibitsAutocomplete.Hide();
                return;
            }
            var search = UrlEncoder.Default.Encode(exhibitName.Text.Trim());
            var (success, res, msg) = await Helper.jsonReq<List<Exhibit>, object>($"exhibits?size=10&search={search}");
            if (!success || res.data == null || res.data.Count == 0)
            {
                exhibitsAutocomplete.Hide();
                return;
            }
            exhibitsAutocomplete.DataSource = res.data;
            exhibitsAutocomplete.Show();
            exhibitsAutocomplete.Focus();
        }

        private void onAutocompleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                var item = exhibitsAutocomplete.SelectedItem as Exhibit;
                if (item == null) return;
                selectedNewExhibit = item;
                exhibitName.Text = item.shortDetail;
                exhibitsAutocomplete.Hide();
            }
            if (e.KeyCode == Keys.Escape)
            {
                exhibitsAutocomplete.DataSource = null;
                exhibitsAutocomplete.Hide();
                selectedNewExhibit = null;
            }
            if (e.KeyCode == Keys.Up && exhibitsAutocomplete.SelectedIndex == 0)
            {
                exhibitName.Focus();
            }
        }

        private void onAddExhibit(object sender, EventArgs e)
        {
            if (selectedNewExhibit == null)
            {
                MessageBox.Show("No new exhibit selected");
                return;
            }
            ownedExhibits.Add(selectedNewExhibit);
            exhibitName.Text = "";
            LocalExhibitRefresh();
        }

        private void onExhibitTableCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
                var item = exhibitTable.CurrentCell.OwningRow.DataBoundItem as Exhibit;
                if (item == null) return;
                if (MessageBox.Show($"Are you sure want to delete {item.shortDetail}?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No) return;
                ownedExhibits.Remove(item);
                LocalExhibitRefresh();
            }
        }
    }
}
