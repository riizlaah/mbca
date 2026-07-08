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
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace MBCA_Desktop.Forms
{
    public partial class AddEditExhibitForm : Form
    {
        private bool editMode { get; set; } = false;
        private Exhibit record { get; set; }
        private string imageUrl { get; set; } = "";
        public AddEditExhibitForm(bool edit, Exhibit? ex = null)
        {
            editMode = edit;
            record = ex ?? new Exhibit();
            InitializeComponent();
            formTitle.Text = (edit ? "Edit" : "Add") + "Exhibit";
            Text = (edit ? "Edit" : "Add") + "Exhibit";
            categories.DisplayMember = "name";
            GetCategories();
        }

        async private Task GetCategories()
        {
            var (success, res, msg) = await Helper.jsonReq<List<ExCategory>, object>("exhibits/categories");
            if (res.data == null || !success)
            {
                MessageBox.Show("Failed to retrieve categories data");
                return;
            }
            categories.DataSource = res.data;
            if (editMode && record != null)
            {
                name.Text = record.name;
                artist.Text = record.artist;
                timePeriod.Text = record.timePeriod;
                categories.SelectedIndex = res.data.FindIndex(p => p.id == record.category.id);
                tags.Text = string.Join(", ", record.tags.Select(e => e.tag));
                image.Image = await Helper.GetImage(record.image);
            }
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
            if (categories.SelectedItem == null)
            {
                MessageBox.Show("Category not selected");
                return;
            }
            if(!editMode && imageUrl == "")
            {
                MessageBox.Show("Image required");
                return;
            }
            var category = categories.SelectedItem as ExCategory;
            if(category == null)
            {
                MessageBox.Show("Category required");
                return;
            }
            try
            {
                using var multipartForm = new MultipartFormDataContent();
                multipartForm.Add(new StringContent(name.Text), "name");
                multipartForm.Add(new StringContent(artist.Text), "artist");
                multipartForm.Add(new StringContent(timePeriod.Text), "timePeriod");
                multipartForm.Add(new StringContent(category.id.ToString()), "categoryId");
                if(imageUrl != "")
                {
                    var bytes = new FileStream(imageUrl, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var content = new StreamContent(bytes);
                    var mimetype = (Path.GetExtension(imageUrl).Trim('.') == "png") ? "image/png" : "image/jpeg";
                    content.Headers.ContentType = new MediaTypeHeaderValue(mimetype);
                    multipartForm.Add(content, "image", Path.GetFileName(imageUrl));
                }
                var tagArr = tags.Text.Split(',').Select(e => e.Trim()).ToList();
                foreach(var tag in tagArr)
                {
                    multipartForm.Add(new StringContent(tag), "tags[]");
                }
                if(editMode)
                {
                    var (success, res, msg) = await Helper.multipartReq($"exhibits/{record.id}", "put", multipartForm);
                    if (!success)
                    {
                        MessageBox.Show(msg, "Error");
                        return;
                    }
                } else
                {
                    var (success, res, msg) = await Helper.multipartReq("exhibits", "post", multipartForm);
                    if(!success)
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
            imageUrl = chooseImage.FileName;
            if (imageUrl == "") return;
            var bytes = File.ReadAllBytes(imageUrl);
            using (var ms = new MemoryStream(bytes))
            {
                image.Image = Image.FromStream(ms);
            }
        }
    }
}
