using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public static class TrainingDatasetHelper
    {
        private static string dir = @"C:\Training\Directions\Sets\";

        //public static void WriteSet(string name, List<List<double>> trainingSet)
        //{
        //    var to = Path.Combine(dir, name);
        //    string line;

        //    using (StreamWriter writer = new System.IO.StreamWriter(to))
        //    {
        //        foreach (var lineValue in trainingSet)
        //        {
        //            line = ListToString(lineValue, " ");
        //            writer.WriteLine(line);
        //        }
        //    }
        //}

        //public static void JoinSet(string name, List<string> objects, int numSets)
        //{
        //    string line;
        //    List<string> lines = new List<string>();
        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        for (int j = 0; j < numSets; j++)
        //        {
        //            var fileName = $"{objects[i]}_{j + 1}.txt";
        //            var path = Path.Combine(dir, fileName);
        //            System.IO.StreamReader file = new System.IO.StreamReader(path);
        //            while ((line = file.ReadLine()) != null)
        //            {
        //                line += GetSetY(i, objects.Count, " ");
        //                lines.Add(line);
        //            }
        //        }
        //    }

        //    var to = Path.Combine(dir, name + ".txt");
        //    using (StreamWriter writer = new System.IO.StreamWriter(to))
        //    {
        //        foreach (var lineValue in lines)
        //        {
        //            writer.WriteLine(lineValue);
        //        }
        //    }
        //}

        public static string GetSetY(int index, int qty, string separator)
        {
            string Y = "";
            for (int j = 0; j < qty; j++)
            {
                if (index == j+1)
                {
                    Y += "1" + separator;
                }
                else
                {
                    Y += "0" + separator;
                }
            }
            return Y;
        }

        public static string ListToString(List<double> values, string separator)
        {
            var stringToReturn = "";
            foreach (var value in values)
            {
                stringToReturn += value.ToString() + separator;
            }
            return stringToReturn;
        }
    }
}
