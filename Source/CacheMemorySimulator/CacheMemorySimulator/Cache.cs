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
                        block.Add(0);
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

        //TODO store function
        public int store(int tag, int set, int line, int block, String rPolicy)
        {
            //Fully Associative
            if (set == -1 && line == -1)
            {

            }
            return -1;
        }

        //TODO load function
        public int load(int tag, int set, int line, int block, String rPolicy)
        {
            int AccessTime = 0;
            //Fully Associative
            if (set == -1 && line == -1)
            {
                Boolean emptySapce = false;
                //Search for a free space in cache
                foreach (List<int> row in this.cache)
                {
                    if (row[0] == 0 || row[4] == block)
                    {
                        //Found an empty space
                        //Write data to row
                        //TODO repl.
                        this.cache[this.cache.IndexOf(row)] = new List<int>(5) { 1, 1, tag, 1, block };
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
                        //Search for line to e replaced
                        foreach (List<int> row in this.cache)
                        {
                            if (row[3] == 000) //found oldest data in cache
                            {
                                if (row[1] == 1) //Data dirty
                                {
                                    //Write data to MM
                                }
                                //Write data to cache
                                List<int> newRow = new List<int>(5) { 1, 1, tag, 111, block };
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
            }
            //Direct Mapping
            else if (set == -1 && line != -1)
            {
                //See if cache line is empty
                if (this.cache[line][0] == 0)
                {
                    //Cache line was empty
                    //Write data to cache
                    //TODO repl.
                    List<int> newRow = new List<int>(5) { 1, 1, tag, 1, block };
                    this.cache[line] = newRow;
                }
                else //Cache line busy
                {
                    if (this.cache[line][1] == 1) //See if data is dirty
                    {
                        //Write data to MM
                        AccessTime = AccessTime + 21;
                    }
                    else
                    {
                        //TODO transfertime
                        List<int> newRow = new List<int>(5) { 1, 1, tag, 1, block };
                        this.cache[line] = newRow;
                    }
                }
            }
            //Set Associative
            else
            {

            }
            return AccessTime;
        }

    }
}
