using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Treasure_island
{
    /// <summary>
    /// Object that contains the weight and value of an item.
    /// </summary>
    struct Item
    {
        #region Members
        private readonly int weight, value;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes an Item object.
        /// </summary>
        /// <param name="weight">The weight of the object.</param>
        /// <param name="value">The value of the object</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when ether the weight or the value is 0 or less.</exception>
        public Item(int weight, int value)
        {
            if (weight <= 0) throw new ArgumentOutOfRangeException("Weight is zero or less", nameof(weight));
            if(value <= 0) throw new ArgumentOutOfRangeException("Value is zero or less", nameof(value));

            this.weight = weight;
            this.value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the weight or the value of the Item.
        /// </summary>
        /// <param name="key">Defines which value to return, the "weight" or the "value".</param>
        /// <returns>The weight or the value of the Item.</returns>
        /// <exception cref="ArgumentException">Thrown when the key is neither "weight", nor "value".</exception>
        public int this[string key]
        {
            get
            {
                if (key == "weight") return this.weight;
                else if (key == "value") return this.value;
                else throw new ArgumentException("Key is neither \"weight\", nor \"value\".", nameof(key));
            }
        }
        #endregion
    }
    internal class Program
    {
        /// <summary>
        /// Read Item objects from a file.
        /// </summary>
        /// <param name="path">Path to the file to read.</param>
        /// <returns>A list of Item objects.</returns>
        public static List<Item> readInput(string path)
        {
            List<Item> items = new List<Item>();
            using (StreamReader sr = new StreamReader(path))
            {
                while(!sr.EndOfStream)
                {
                    string[] split = sr.ReadLine().Split(' ');
                    items.Add(
                        new Item(
                            Convert.ToInt32(split[0]),
                            Convert.ToInt32(split[1])
                            )
                        );
                }
            }

            return items;
        }

        static void Main(string[] args)
        {
        }
    }
}
