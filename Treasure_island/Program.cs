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
        /// <param name="key">Defines which memeber to return, the "weight" or the "value".</param>
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

        #region Functions
        public override string ToString()
        {
            return String.Format("Item(\"weight\": {0}, \"value\": {1})", this.weight, this.value);
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
            // Read input from a file
            List<Item> items = readInput("../../Input/input_1.txt");
            int totalWeight = 0, totalItems = 0;
            int[,] dynamic_matrix;

            // Get the number of items and the sum of their weights
            foreach (Item item in items) 
            {
                totalWeight += item["weight"];
                totalItems += 1;
            }

            // Create matrix where the number of rows is the number of items,
            // and the number of columns is the sum of the weights.
            dynamic_matrix = new int[totalItems, totalWeight];
            dynamic_matrix[0, items.ElementAt(0)["weight"] - 1] = items.ElementAt(0)["value"];

            List<int> indeciesToCheck = new List<int>();
            indeciesToCheck.Add(items.ElementAt(0)["weight"]);


            for (int i = 1; i < items.Count; i++)
            {
                for (int j = 0; j < indeciesToCheck.Count; j++)
                {
                    dynamic_matrix[i, indeciesToCheck.ElementAt(j) - 1] = dynamic_matrix[i - 1, indeciesToCheck.ElementAt(j) - 1];
                }

                Item item = items.ElementAt(i);

                if (dynamic_matrix[i - 1, item["weight"] - 1] < item["value"])
                {
                    dynamic_matrix[i, item["weight"] - 1] = item["value"];
                }

                for (int j = 0; j < indeciesToCheck.Count; j++)
                {
                    if (
                        dynamic_matrix[i - 1, indeciesToCheck.ElementAt(j) + item["weight"] - 1] <
                        dynamic_matrix[i - 1, indeciesToCheck.ElementAt(j) - 1] + item["value"]
                        )
                    {
                        dynamic_matrix[i, indeciesToCheck.ElementAt(j) + item["weight"] - 1] = dynamic_matrix[i - 1, indeciesToCheck.ElementAt(j) - 1] + item["value"];
                    }
                }

                int indeciesCount = indeciesToCheck.Count;
                for (int j = 0; j < indeciesCount; j++)
                {
                    if (!indeciesToCheck.Contains(indeciesToCheck.ElementAt(j) + item["weight"]))
                        indeciesToCheck.Add(indeciesToCheck.ElementAt(j) + item["weight"]);
                }
                if (!indeciesToCheck.Contains(item["weight"])) indeciesToCheck.Add(item["weight"]);
            }

            // Print out dynamic table
            //string top = "┌";
            //string bottom = "└";
            //string middle = "├";
            //for (int i = 0; i < dynamic_matrix.GetLength(1) - 1; i++)
            //{
            //    top += "────┬";
            //    bottom += "────┴";
            //    middle += "────┼";
            //}
            //top += "────┐";
            //bottom += "────┘";
            //middle += "────┤";
            //
            //Console.WriteLine(top);
            //for (int i = 0; i < dynamic_matrix.GetLength(0); i++)
            //{
            //    Console.Write("│");
            //    for (int j = 0; j < dynamic_matrix.GetLength(1); j++)
            //    {
            //        Console.Write(" {0, 2} │", dynamic_matrix[i, j]);
            //    }
            //    Console.WriteLine();
            //    if (i < dynamic_matrix.GetLength(0) - 1) Console.WriteLine(middle);
            //}
            //Console.WriteLine(bottom);

            Console.Write("Maximum súly: ");
            int max = Convert.ToInt32(Console.ReadLine());
            if (max > dynamic_matrix.GetLength(1)) max = dynamic_matrix.GetLength(1);
            int maxIndex = 0;

            // Legnagyobb érték (maximum súly melletti) keresés
            for (int i = 1; i < max; i++)
            {
                if (dynamic_matrix[dynamic_matrix.GetLength(0) - 1, i] > dynamic_matrix[dynamic_matrix.GetLength(0) - 1, maxIndex]) maxIndex = i;
            }

            Console.WriteLine("Elhozott tárgyak:");
            Console.WriteLine("Tömeg│Érték");
            Console.WriteLine("─────┼─────");

            // Elhozott tárgyak kiíratása
            for (int i = items.Count - 1; i > 0; i--)
            {
                if (maxIndex >= 0 && dynamic_matrix[i, maxIndex] != dynamic_matrix[i - 1, maxIndex])
                {
                    Item item = items.ElementAt(i);
                    Console.WriteLine("{0, 5}│{1, 5}", item["weight"], item["value"]);
                    maxIndex -= item["weight"];
                }
            }
            // Utolsó elem ellenőrzése
            if (maxIndex >= 0 && dynamic_matrix[0, maxIndex] > 0)
                Console.WriteLine("{0, 5}│{1, 5}", items.ElementAt(0)["weight"], items.ElementAt(0)["value"]);

            Console.ReadLine();
        }
    }
}
