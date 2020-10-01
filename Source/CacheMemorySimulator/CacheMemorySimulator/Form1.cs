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
using System.Threading;
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
            materialSkinManager.ColorScheme = new ColorScheme(Primary.LightBlue600, Primary.LightBlue700, Primary.LightBlue300, Accent.DeepOrange100, TextShade.WHITE);
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
        //TODO FIX buttongroup>2
        private void selectButton(MaterialButton Btn)
        {
            if (Btn.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                Btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            }
            else if (Btn.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained)
            {
                Btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            }
        }
        //TODO ButtonGroupSwitch
        private void ButtonGroupSwitch(int GroupCode)
        {
            switch (GroupCode)
            {
                case 1:
                    selectButton(this.materialButton2);
                    selectButton(this.materialButton3);
                    break;
                case 2:
                    selectButton(this.materialButton4);
                    selectButton(this.materialButton5);
                    break;
                case 3:
                    selectButton(this.materialButton6);
                    selectButton(this.materialButton7);
                    selectButton(this.materialButton8);
                    selectButton(this.materialButton9);
                    break;
                case 4:
                    selectButton(this.materialButton10);
                    selectButton(this.materialButton11);
                    break;
                case 5:
                    selectButton(this.materialButton12);
                    selectButton(this.materialButton13);
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
                ListViewItem item0 = new ListViewItem("Busy", 0);
                ListViewItem item1 = new ListViewItem("Dirty", 0);
                ListViewItem item2 = new ListViewItem("Tag", 0);
                ListViewItem item3 = new ListViewItem("Rerp", 0);
                ListViewItem item4 = new ListViewItem("Data", 0);
                for (int i = 0; i < 5; i++)
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
                CacheRep.Items.AddRange(new ListViewItem[] { item0, item1, item2, item3, item4 });
            }
        }
        public void send()
        {
            try
            {
                int address = int.Parse(input.Text);

                this.interpretAddress(address, 4, 32, 8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void interpretAddress(int address, int wordSize, int blockSize, int setSize)
        {
            try
            {
                int line, tag, set;

                int word = address / wordSize;
                int num_words_block = blockSize / wordSize;
                int block = word / num_words_block;
                int word_in_block = word % num_words_block;
                int num_lines = CH.getCacheLines();
                int num_sets_cache = num_lines / setSize;

                //Calculate Set,Tag,Line
                switch (setSize)
                {
                    case 1:
                        //Direct Mapping
                        tag = block / num_lines;
                        line = block % num_lines;
                        this.Chtag.Text = tag.ToString();
                        break;
                    case 2:
                        //Set Associative
                        tag = block / num_sets_cache;
                        set = block % num_sets_cache;
                        this.Chtag.Text = tag.ToString();
                        this.set.Text = set.ToString();
                        break;
                    case 4:
                        //Set Associative
                        tag = block / num_sets_cache;
                        set = block % num_sets_cache;
                        this.Chtag.Text = tag.ToString();
                        this.set.Text = set.ToString();
                        break;
                    case 8:
                        //Fully Associative
                        tag = address;
                        this.Chtag.Text = tag.ToString();
                        break;
                }



                this.block.Text = block.ToString();
                this.address.Text = address.ToString();
                this.word.Text = word.ToString();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void materialButton13_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(5);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(1);
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(1);
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(2);
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(2);
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(3);
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(3);
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(3);
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(3);
        }

        private void materialButton11_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(4);
        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(4);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.send();

        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            this.ButtonGroupSwitch(5);
        }
    }
}
