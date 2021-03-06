﻿using MaterialSkin;
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
        private int TotalAccesTime = 0;
        private int hits = 0;
        private int misses = 0;

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
            this.reload();

        }
        private void selectButton(MaterialButton Btn)
        {
            Btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
        }
        private void reload()
        {
            //VALUES
            this.TotalAccesTime = 0;
            this.hits = 0;
            this.misses = 0;

            //CACHE RELOAD
            this.CH.initilaize();
            this.loadTable(this.CH.getCache());

            //LABELS
            this.address.Text = "-";
            this.word.Text = "-";
            this.block.Text = "-";
            this.set.Text = "-";
            this.line.Text = "-";
            this.Chtag.Text = "-";
            this.mapping.Text = "-";
            this.hitmiss.Text = "-";
            this.AccessTime.Text = "-";
            this.h.Text = "-";
            this.TAccesTime.Text = "-";
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
        private void SetHeight(ListView listView, int height)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, height);
            listView.SmallImageList = imgList;
        }
        private void loadTable(List<List<int>> List)
        {
            //Clear ListView
            CacheRep.Items.Clear();
            //Set proper size
            SetHeight(this.CacheRep, 35);

            foreach (List<int> row in List)
            {
                string[] listRow = new string[5];
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] < 0)
                    {
                        listRow[i] = "-";
                    }
                    else
                    {
                        listRow[i] = row[i].ToString();
                    }
                }
                this.CacheRep.Items.Add(new ListViewItem(listRow));
            }
        }
        public void send()
        {
            try
            {
                int address = int.Parse(input.Text);
                int tag, set, line, block, num_words, num_sets;

                //INTERPRET ADDRESS
                (tag, set, line, block, num_words, num_sets) = interpretAddress(address, this.wSize, this.bSize, this.sSize);
                int AT;
                String h = "-";

                //OPERATE
                if (this.operation == "STORE")
                {
                    //STORE TO MM
                    AT = this.CH.store(tag, set, line, block, this.rPolicy, num_words, num_sets);
                }
                else //this.operation == "LOAD"
                {
                    //LOAD CACHE
                    (AT, h) = this.CH.load(tag, set, line, block, this.rPolicy, num_words, num_sets);

                }
                //GET HITS / MISSES
                if (h == "hit")
                {
                    this.hits++;
                }
                else if (h == "miss")
                {
                    misses++;
                }

                //GET TOTAL ACCESS TIME
                this.TotalAccesTime += AT;

                //LOAD VALUES TO OUTPUT
                this.TAccesTime.Text = this.TotalAccesTime.ToString();
                this.AccessTime.Text = AT.ToString();
                this.hitmiss.Text = h;
                this.h.Text = hits.ToString() + "/" + misses.ToString();
                this.loadTable(this.CH.getCache());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public (int, int, int, int, int, int) interpretAddress(int address, int wordSize, int blockSize, int setSize)
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
                    //Fully Associative
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
                    //Set Associative
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

                return (tag, set, line, block, num_words_block, num_sets_cache);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (-1, -1, -1, -1, -1, -1);
            }

        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
            if (wSize4btn.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.wSize = 4;
                this.ButtonGroupSwitch(1);
                wSize4btn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }

        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (materialButton3.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.wSize = 8;
                this.ButtonGroupSwitch(1);
                materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            if (materialButton5.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.bSize = 32;
                this.ButtonGroupSwitch(2);
                materialButton5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (materialButton4.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.bSize = 64;
                this.ButtonGroupSwitch(2);
                materialButton4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            if (materialButton7.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.sSize = 1;
                this.ButtonGroupSwitch(3);
                materialButton7.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            if (materialButton6.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.sSize = 2;
                this.ButtonGroupSwitch(3);
                materialButton6.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            if (materialButton9.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.sSize = 4;
                this.ButtonGroupSwitch(3);
                materialButton9.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            if (materialButton8.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.sSize = 8;
                this.ButtonGroupSwitch(3);
                materialButton8.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton11_Click(object sender, EventArgs e)
        {
            if (materialButton11.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.rPolicy = "FIFO";
                this.ButtonGroupSwitch(4);
                materialButton11.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            if (materialButton10.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.rPolicy = "LRU";
                this.ButtonGroupSwitch(4);
                materialButton10.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
                this.reload();
            }
        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            if (materialButton12.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.operation = "STORE";
                this.ButtonGroupSwitch(5);
                materialButton12.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            }
        }
        private void materialButton13_Click(object sender, EventArgs e)
        {
            if (materialButton13.Type == MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined)
            {
                this.operation = "LOAD";
                this.ButtonGroupSwitch(5);
                materialButton13.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            }
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.send();

        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.send();
            }
        }

        private void materialLabel16_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Asiern/CM-SimulatorGUI");
        }
    }
}
