using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CacheMemorySimulator
{
    class Cache
    {
        private List<List<int>> cache;
        private int bCount;
        private int num_lines = 8;

        private int TMM = 21;
        private int TCM = 2;
        private int TBUFF = 1;


        public Cache(int wSize, int bSize)
        {
            this.bCount = bSize / wSize;
            this.cache = new List<List<int>>(bCount);

            this.initilaize();
        }

        public void initilaize()
        {
            try
            {
                this.cache.Clear();
                for (int blockIndex = 0; blockIndex < this.bCount; blockIndex++)
                {
                    List<int> block = new List<int>(5);
                    for (int wordIndex = 0; wordIndex < 5; wordIndex++)
                    {
                        block.Add(-1);
                    }
                    this.cache.Add(block);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public List<int> getCacheBlock(int index)
        {
            try
            {
                return this.cache[index];
            }
            catch (IndexOutOfRangeException IE)
            {
                MessageBox.Show(IE.Message);
            }
            return null;
        }
        public List<List<int>> getCache()
        {
            try
            {
                return this.cache;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
            return null;
        }

        public int getCacheLines()
        {
            return this.num_lines;
        }
        public int num_used_lines()
        {
            int value = 0;
            foreach (List<int> row in this.cache)
            {
                if (row[0] == 1)
                {
                    value++;
                }
            }
            return value;
        }
        private int indexOfBlock(int Block)
        {
            foreach (List<int> row in this.cache)
            {
                if (row[4] == Block)
                {
                    return this.cache.IndexOf(row);
                }
            }
            return -1;
        }
        private void updateRPIndex()
        {
            foreach (List<int> row in this.cache)
            {
                this.cache[this.cache.IndexOf(row)][3] = row[3] - 1;
            }
        }
        private void updateRPIndex(int t, int b)
        {
            for (int i = b; i <= t; i++)
            {
                this.cache[i][3] = this.cache[i][3] - 1;
            }
        }
        public int store(int tag, int set, int line, int block, String rPolicy, int num_words, int num_sets)
        {
            Boolean found = false;
            int AccessTime = 0;
            //Fully Associative
            if (set == -1 && line == -1)
            {

                foreach (List<int> row in this.cache)
                {
                    if (row[4] == block)
                    {
                        //Set dirty to 0
                        this.cache[this.cache.IndexOf(row)][1] = 0;
                        found = true;

                        //Move data CM => MM
                        //Tbt = Tmm + (num_words -1)Tbuff
                        AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                        break;
                    }
                }

            }
            //Direct Mapping
            else if (set == -1 && line != -1)
            {

                foreach (List<int> row in this.cache)
                {
                    if (row[4] == block)
                    {
                        //Set dirty to 0
                        this.cache[this.cache.IndexOf(row)][1] = 0;
                        found = true;

                        //Move data CM => MM
                        //Tbt = Tmm + (num_words -1)Tbuff
                        AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                        break;
                    }

                }
            }
            //Set Associative
            else
            {
                int num_lines_set = num_lines / num_sets;
                int top = num_lines_set + (set * num_lines_set) - 1;
                int bottom = 0 + num_lines_set * set;

                for (int i = bottom; i <= top; i++)
                {
                    if (this.cache[i][4] == block)
                    {
                        this.cache[i][1] = 0;
                        found = true;

                        //Move data CM => MM
                        //Tbt = Tmm + (num_words -1)Tbuff
                        AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                        break;
                    }
                }

            }
            if (!found)
            {
                MessageBox.Show("Block not found on cache. Can't store data in MM.");
            }
            return AccessTime;
        }

        public (int, string) load(int tag, int set, int line, int block, String rPolicy, int num_words, int num_sets)
        {
            String h = "";
            int AccessTime = 0;
            //Fully Associative
            if (set == -1 && line == -1)
            {
                //Search for block on cache
                if (this.indexOfBlock(block) != -1)
                {
                    //Set dirty to 1
                    this.cache[this.indexOfBlock(block)][1] = 1;
                    //Block is already on cache => hit
                    h = "hit";
                    AccessTime = this.TCM;
                }
                else
                {
                    Boolean emptySapce = false;
                    //Search for a free space in cache
                    foreach (List<int> row in this.cache)
                    {
                        if (row[0] == -1 || row[0] == 0)
                        {
                            //Found an empty space
                            this.cache[this.cache.IndexOf(row)] = new List<int>(5) { 1, 1, tag, this.num_used_lines(), block };
                            emptySapce = true;
                            break;
                        }
                    }
                    //No empty space found on cache
                    if (!emptySapce)
                    {
                        //Need to rewrite data
                        if (rPolicy == "FIFO")
                        {
                            //Search for line to be replaced
                            foreach (List<int> row in this.cache)
                            {
                                if (row[3] == 0) //found oldest data in cache
                                {
                                    if (row[1] == 1) //Data dirty
                                    {
                                        //Write data to MM
                                        //Tbt = Tmm + (num_words -1)Tbuff
                                        AccessTime += this.TMM + this.TBUFF * (num_words - 1);
                                    }
                                    //Write data to cache
                                    List<int> newRow = new List<int>(5) { 1, 1, tag, this.num_used_lines(), block };
                                    this.cache[this.cache.IndexOf(row)] = newRow;
                                    //Modify repl.
                                    this.updateRPIndex();
                                    AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Search for line to be replaced
                            foreach (List<int> row in this.cache)
                            {
                                if (row[3] == 0) //least recently used data in cache
                                {
                                    if (row[1] == 1) //Data dirty
                                    {
                                        //Write data to MM
                                        //Tbt = Tmm + (num_words -1)Tbuff
                                        AccessTime += this.TMM + this.TBUFF * (num_words - 1);
                                    }
                                    //Write data to cache
                                    List<int> newRow = new List<int>(5) { 1, 1, tag, this.num_used_lines(), block };
                                    this.cache[this.cache.IndexOf(row)] = newRow;
                                    //Modify repl.
                                    this.updateRPIndex();
                                    AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                                    break;
                                }
                            }

                        }
                    }
                    h = "miss";
                    //Access Time = TCM + Tbt
                    //Tbt = Tmm + (num_words -1)Tbuff
                    AccessTime += this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                }
            }
            //Direct Mapping
            else if (set == -1 && line != -1)
            {
                //See if block is already on cache
                if (this.cache[line][4] == block)
                {
                    //Set dirty to 1
                    this.cache[line][1] = 1;
                    h = "hit";
                    AccessTime = this.TCM;
                }
                //See if cache line is empty
                else if (this.cache[line][0] == 0)
                {
                    //Cache line was empty
                    //Write data to cache
                    List<int> newRow = new List<int>(5) { 1, 1, tag, 0, block };
                    this.cache[line] = newRow;
                    h = "miss";
                    //Tbt = Tmm + (num_words -1)Tbuff
                    AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                }
                else //Cache line busy
                {
                    if (this.cache[line][1] == 1) //See if data is dirty
                    {
                        //Tbt = Tmm + (num_words -1)Tbuff
                        AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                        List<int> newRow = new List<int>(5) { 1, 1, tag, 0, block };
                        this.cache[line] = newRow;
                    }
                    else
                    {
                        List<int> newRow = new List<int>(5) { 1, 1, tag, 0, block };
                        this.cache[line] = newRow;
                        //Tbt = Tmm + (num_words -1)Tbuff
                        AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);

                    }
                    h = "miss";
                }
            }
            //Set Associative
            else
            {
                int num_lines_set = num_lines / num_sets;
                int top = num_lines_set + (set * num_lines_set) - 1;
                int bottom = 0 + num_lines_set * set;

                Boolean found = false;

                //Search for block on cache
                for (int i = bottom; i <= top; i++)
                {
                    //Get line
                    List<int> row = this.getCacheBlock(i);
                    //Block found
                    if (row[4] == block)
                    {
                        //LRU update index
                        if (rPolicy == "LRU")
                        {
                            this.updateRPIndex(top, bottom);
                            this.cache[i][3] = num_lines_set - 1;
                        }
                        this.cache[i][1] = 1;
                        h = "hit";
                        AccessTime = this.TCM;
                        found = true;
                        break;
                    }
                }
                //If Block was not on cache
                if (!found)
                {
                    Boolean empty = false;
                    //Search for an empty line
                    for (int i = bottom; i <= top; i++)
                    {
                        if (this.cache[i][0] <= 0)
                        {
                            this.cache[i] = new List<int> { 1, 1, tag, num_lines_set, block };
                            //Tbt = Tmm + (num_words -1)Tbuff
                            AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                            empty = true;
                            this.updateRPIndex(top, bottom);
                            break;
                        }
                    }
                    //No empty lines found
                    if (!empty)
                    {
                        //Search for line to be replaced in cache
                        for (int i = bottom; i <= top; i++)
                        {
                            if (this.cache[i][3] == 0)
                            {
                                this.cache[i] = new List<int> { 1, 1, tag, num_lines_set, block };
                                //Tbt = Tmm + (num_words -1)Tbuff
                                AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                                empty = true;
                                this.updateRPIndex(top, bottom);
                                break;
                            }
                        }

                    }
                    h = "miss";
                }

            }
            return (AccessTime, h);
        }
    }
}
