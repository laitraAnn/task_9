using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp.Forms
{
    public partial class MainWindow : Form
    {
        private int _itemCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string userRole, string name)
        {
            InitializeComponent();
            stripRole.Text += userRole;
            stripName.Text += name;
            LoadAndInitData();

            var CategoryType = Program.context.Category.OrderBy(p => p.CategoryName).ToList();
            CategoryType.Insert(0, new Category{ CategoryName = "Все типы"});
            comboCategory.DataSource = CategoryType;
            comboCategory.DisplayMember = "CategoryName";
            comboCategory.ValueMember = "CategoryId";
        }

        private void LoadAndInitData()
        {
            var currentGoods = Program.context.Good.Join(Program.context.Category, p => p.CategoryId, t => t.CategoryId, (p, t) => new { p.GoodId, p.GoodName, p.Price, p.Picture, p.Description, t.CategoryName, p.CategoryId}).ToList();

            dgvGoods.DataSource = currentGoods;
            dgvGoods.Columns[6].Visible = false;

            dgvGoods.Columns[0].HeaderText = "Артикул товара";
            dgvGoods.Columns[1].HeaderText = "Название товара";
            dgvGoods.Columns[2].HeaderText = "Цена";
            dgvGoods.Columns[3].HeaderText = "Изображение";
            dgvGoods.Columns[4].HeaderText = "Описание";
            dgvGoods.Columns[5].HeaderText = "Категория";

            _itemCount = dgvGoods.Rows.Count;
            labelCountGood.Text = $"Результат запроса: ({currentGoods.Count} записей из {_itemCount})";
        }

        private void UpdateData()
        {
            var currentGoods = Program.context.Good.Join(Program.context.Category, p => p.CategoryId, t => t.CategoryId,
                (p, t) => new { p.GoodId, p.GoodName, p.Price, p.Description, t.CategoryName, p.CategoryId }).ToList();

            if (comboCategory.SelectedIndex > 0)
                currentGoods = currentGoods.Where(y => y.CategoryId == (comboCategory.SelectedItem as Category).CategoryId).ToList();

            currentGoods = currentGoods.Where(p => p.GoodName.ToLower().Contains(txtNameGood.Text.ToLower())).ToList();

            if (ComboSort.SelectedIndex >= 0)
            {
                if (ComboSort.SelectedIndex == 0)
                    currentGoods = currentGoods.OrderBy(p => p.Price).ToList();

                else if (ComboSort.SelectedIndex == 1)
                    currentGoods = currentGoods.OrderByDescending(p => p.Price).ToList();

                else if (ComboSort.SelectedIndex == 2)
                    currentGoods = currentGoods.OrderBy(p => p.GoodName).ToList();

                else if (ComboSort.SelectedIndex == 3)
                    currentGoods = currentGoods.OrderByDescending(p => p.GoodName).ToList();
            }

            dgvGoods.DataSource = currentGoods;
            labelCountGood.Text = $" Результат запроса: {currentGoods.Count} записей из {_itemCount}";
        }

        private void ComboSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void comboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void txtNameGood_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void dgvGoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = (int)dgvGoods[0, e.RowIndex].Value;
            var product = Program.context.Good.Where(p => p.GoodId == a).FirstOrDefault();

            labelNameGood.Text = product.GoodName.ToString();
            labelPrice.Text = product.Price.ToString();

            if (string.IsNullOrWhiteSpace(product.Picture))
            {
                string placeholderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "picture.jpg");
                pictureGood.Image = Image.FromFile(placeholderPath);
            }
            else
            {
                string imageName = product.Picture.ToString(); 
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);
                pictureGood.Image = Image.FromFile(imagePath);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }
    }
}
