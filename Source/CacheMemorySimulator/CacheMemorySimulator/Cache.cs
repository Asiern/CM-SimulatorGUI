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
        //TODO store function
        public int store(int tag, int set, int line, int block, String rPolicy, int num_words)
        {
            int AccessTime = 0;
            //Fully Associative
            if (set == -1 && line == -1)
            {

            }
            //Direct Mapping
            else if (set == -1 && line != -1)
            {
                Boolean found = false;
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
                    }
                }
                if (!found)
                {
                    MessageBox.Show("Block not found on cache. Can't store data.");
                }
            }
            //Set Associative
            else
            {

            }
            return AccessTime;
        }

        //TODO load function
        public (int, string) load(int tag, int set, int line, int block, String rPolicy, int num_words)
        {
            String h = "";
            int AccessTime = 0;
            //Fully Associative
            if (set == -1 && line == -1)
            {
                //Search for block on cache
                if (this.indexOfBlock(block) != -1)
                {
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
                            //TODO caclculate repl.
                            this.cache[this.cache.IndexOf(row)] = new List<int>(5) { 1, 1, tag, 7, block };
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
                            //TODO replace using fifo
                            //Search for line to be replaced
                            foreach (List<int> row in this.cache)
                            {
                                if (row[3] == 0) //found oldest data in cache
                                {
                                    if (row[1] == 1) //Data dirty
                                    {
                                        //Write data to MM
                                    }
                                    //Write data to cache
                                    //Modify repl.
                                    this.updateRPIndex();
                                    List<int> newRow = new List<int>(5) { 1, 1, tag, 7, block };
                                    this.cache[this.cache.IndexOf(row)] = newRow;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //TODO replace using LRU
                        }
                    }
                    h = "miss";
                    //Tbt = Tmm + (num_words -1)Tbuff
                    AccessTime = this.TCM + this.TMM + this.TBUFF * (num_words - 1);
                }
            }
            //Direct Mapping
            else if (set == -1 && line != -1)
            {
                //See if block is already on cache
                if (this.cache[line][4] == block)
                {
                    h = "hit";
                    AccessTime = this.TCM;
                }
                //See if cache line is empty
                else if (this.cache[line][0] == 0)
                {
                    //Cache line was empty
                    //Write data to cache
                    List<int> newRow = new List<int>(5) { 1, 1, tag, 1, block };
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
                        List<int> newRow = new List<int>(5) { 1, 1, tag, 1, block };
                        this.cache[line] = newRow;
                    }
                    else
                    {
                        List<int> newRow = new List<int>(5) { 1, 1, tag, 1, block };
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

            }
            return (AccessTime, h);
        }

    }
}
