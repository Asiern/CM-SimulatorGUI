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
            //Clear ListView
            CacheRep.Clear();
            //TODO separate blocks into Table rows
            foreach (List<int> Block in List)
            {
                ListViewItem item0 = new ListViewItem("Busy",0);
                ListViewItem item1 = new ListViewItem("Dirty", 0);
                ListViewItem item2 = new ListViewItem("Tag", 0);
                ListViewItem item3 = new ListViewItem("Rerp", 0);
                ListViewItem item4 = new ListViewItem("Data", 0);
                for (int i=0;i<5;i++)
                {
                    switch (i)
                    {
                        case 0:
                            item0.SubItems.Add(Block[i].ToString());
                            break;
                        case 1:
                            item1.SubItems.Add(Block[i].ToString());
                            break;
                        case 2:
                            item2.SubItems.Add(Block[i].ToString());
                            break;
                        case 3:
                            item3.SubItems.Add(Block[i].ToString());
                            break;
                        case 4:
                            item4.SubItems.Add(Block[i].ToString());
                            break;

                    }
                }
                CacheRep.Items.AddRange(new ListViewItem[] { item0,item1,item2,item3,item4});
            } 
        }
        private void materialButton13_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(1, this.materialButton13);
        }

    }
}
