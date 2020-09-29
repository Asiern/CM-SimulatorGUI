using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CacheMemorySimulator
{
    public partial class Form1 : MaterialForm
    {
        Cache CH;
        double[][] datosCache;
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TODO get default values

            //Initialize Cache
            //Default values
            CH = new Cache(4, 32, 1, "FIFO");
            //Load Cache Table
            loadTable(CH.getCache());

        }
        //TODO selectButton
        private void selectButton(MaterialButton Btn, Boolean status)
        {
            Btn.Enabled = false;
        }
        //TODO ButtonGroupSwitch
        private void ButtonGroupSwitch(int GroupCode, MaterialButton btn)
        {
            switch (GroupCode)
            {
                case 1:
                    selectButton(btn,true);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        private void loadTable(List<List<int>> List)
        {
            foreach(List<int> Block in List)
            {
                foreach(int value in Block)
                {
                    var item = new ListViewItem(value.ToString());
                    CacheRep.Items.Add(item);
                }
            }
        }
        private void materialButton13_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(1, this.materialButton13);
        }

        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
