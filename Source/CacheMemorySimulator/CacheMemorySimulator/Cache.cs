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

        //TODO operate function
        public void store(int tag, int set, int line, int block, String rPolicy)
        {
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
                    }
                    else
                    {
                        //TODO replace using LRU
                    }
                }
            }
        }

    }
}
