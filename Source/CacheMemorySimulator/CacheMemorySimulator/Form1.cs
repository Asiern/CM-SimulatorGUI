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
        //Default values
        private int wSize = 4;
        private int bSize = 32;
        private int sSize = 1;
        private String rPolicy = "FIFO";
        private String operation = "LOAD";

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
            //Initialize Cache
            //Default values
            CH = new Cache(this.wSize, this.bSize);
            //Load Cache Table
            loadTable(CH.getCache());

        }
        private void selectButton(MaterialButton Btn)
        {
            Btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
        }
        private void ButtonGroupSwitch(int GroupCode)
        {
            switch (GroupCode)
            {
                case 1:
                    selectButton(this.wSize4btn);
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
                int tag, set, line, block;
                (tag, set, line, block) = interpretAddress(address, this.wSize, this.bSize, this.sSize);

                this.CH.operate(this.operation, tag, set, line, block, this.rPolicy);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public (int, int, int, int) interpretAddress(int address, int wordSize, int blockSize, int setSize)
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


                if (num_sets_cache == num_lines)
                {
                    //Direct
                    tag = block / num_lines;
                    line = block % num_lines;
                    set = -1;
                    this.Chtag.Text = tag.ToString();
                    this.set.Text = "None";
                    this.line.Text = line.ToString();
                    this.mapping.Text = "Direct";
                }
                else if (num_sets_cache == 1)
                {
                    //Fully
                    tag = block;
                    line = -1;
                    set = -1;
                    this.line.Text = "None";
                    this.set.Text = "None";
                    this.Chtag.Text = tag.ToString();
                    this.mapping.Text = "Fully";
                }
                else
                {
                    //Set 
                    tag = block / num_sets_cache;
                    set = block % num_sets_cache;
                    line = -1;
                    this.line.Text = "None";
                    this.Chtag.Text = tag.ToString();
                    this.set.Text = set.ToString();
                    this.mapping.Text = "Set";
                }

                this.block.Text = block.ToString();
                this.address.Text = address.ToString();
                this.word.Text = word.ToString();

                return (tag, set, line, block);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (-1, -1, -1, -1);
            }

        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.wSize = 4;
            this.ButtonGroupSwitch(1);
            wSize4btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            this.wSize = 8;
            this.ButtonGroupSwitch(1);
            materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            this.bSize = 32;
            this.ButtonGroupSwitch(2);
            materialButton5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            this.bSize = 64;
            this.ButtonGroupSwitch(2);
            materialButton4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            this.sSize = 1;
            this.ButtonGroupSwitch(3);
            materialButton7.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            this.sSize = 2;
            this.ButtonGroupSwitch(3);
            materialButton6.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            this.sSize = 4;
            this.ButtonGroupSwitch(3);
            materialButton9.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            this.sSize = 8;
            this.ButtonGroupSwitch(3);
            materialButton8.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton11_Click(object sender, EventArgs e)
        {
            this.rPolicy = "FIFO";
            this.ButtonGroupSwitch(4);
            materialButton11.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            this.rPolicy = "LRU";
            this.ButtonGroupSwitch(4);
            materialButton10.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            this.operation = "STORE";
            this.ButtonGroupSwitch(5);
            materialButton12.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }
        private void materialButton13_Click(object sender, EventArgs e)
        {
            this.rPolicy = "LOAD";
            this.ButtonGroupSwitch(5);
            materialButton13.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.send();

        }
    }
}
