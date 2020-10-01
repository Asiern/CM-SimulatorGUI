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
        private int wSize, bSize, bCount, sSize;
        private String rPolicy;
        private int num_lines = 8;


        public Cache(int wSize, int bSize, int sSize, String rPolicy)
        {
            this.wSize = wSize;
            this.bSize = bSize;
            this.sSize = sSize;
            this.rPolicy = rPolicy;
            this.bCount = bSize / wSize;
            this.cache = new List<List<int>>(bCount);

            this.initilaize();
        }

        private void initilaize()
        {
            try
            {
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

        private void operate(int address, Boolean operation)
        {
            if (operation)
            {
                //TODO operate Write data
            }
            else
            {
                //TODO operate Read data
            }
        }

        //TODO CACHE Getters/Setters
    }
}
