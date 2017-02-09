using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;
using FileWriter;

public class TempCSV : MonoBehaviour {

	
	void Start ()
    {

        List<List<string>> dataGrid = CsvFileReader.ReadAll("test.csv", Encoding.GetEncoding("gbk"));

        // TODO: deal with data grid
        foreach (var line in dataGrid)
        {
            foreach (var cell in line)
            {
                Console.Write("\t\t" + cell);
            }
            Console.Write("\n");
        }

        CsvFileWriter.WriteAll(dataGrid, "output2.csv", Encoding.GetEncoding("gbk"));

    List<string> row = new List<string>();
        using (var reader = new CsvFileReader("Test.csv"))
        {
            while (reader.ReadRow(row))
            {
                // TODO: Do something with columns' values
                foreach (string cell in row)
                {
                    Console.Write("\t\t" + cell);
                }
                Console.Write("\n");
            }
        }

    using (var writer = new CsvFileWriter("Test.csv"))
        {
            // Write each row of data
            for (int i = 0; i < 10; i++)
            {
                List<string> newline = new List<string>();

                // TODO: Populate column values for this row
                for (int j = 0; j < 5; j++)
                {
                    newline.Add("" + j);
                }

                writer.WriteRow(newline);
            }

        }
    }
}
