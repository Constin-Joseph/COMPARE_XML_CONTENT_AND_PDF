using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            string filePath;
            DirectoryInfo d = new DirectoryInfo(@"E:\cats project\CEDR_I_71_06_P");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.pdf"); //Getting Text files          
            string str = "";
            var value = "";
            var resultString = "";
            List<PageDetails> Pagedetails = new List<PageDetails>();

            Dictionary<string, string> PageDetails = new Dictionary<string, string>();

            for (int i = 0; i < Files.Length; i++)
            {
                str =  Files[i].FullName.ToString();
                filePath = str;               
               string strText = string.Empty;
                     try
                { 
                    PdfReader reader = new PdfReader(filePath);                   
                  for (int page = 2; page <= 2; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                        strText = PdfTextExtractor.GetTextFromPage(reader, page, its);

                        string TotalPages = reader.NumberOfPages.ToString();
                        strText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(strText)));
                        
                        string[] lines = strText.Split('\n');                      
                        int address = Convert.ToInt32(Regex.Match(lines[0], @"\d+").Value) - 1;

                        string firstpageno = address.ToString();

                        int lastpagenostr = address + reader.NumberOfPages - 1;

                        string lastpageno = lastpagenostr.ToString();
                        
                        Pagedetails.Add(new PDFApp2.PageDetails { firstpage = firstpageno, LastPage = lastpageno, Pagecount = TotalPages });
                    }
                    reader.Close();
                }
                
                catch (Exception ex)
                {
                    //Console.WriteLine("Error");
                }                                  
            }                      
            List<string> add_list = new List<string>();
            List<string> add_list1 = new List<string>();
            for (int i = 0; i < Files.Length; i++)
            {
                str = "E:/cats project/CEDR_I_71_06_P/" + Files[i];
                
                value = Files[i].ToString();
                
                resultString = Regex.Match(value, @"\d+").Value;
                add_list.Add(resultString);                
            }

            DataSet ds = new DataSet();
            ds.ReadXml(@"E:\cats project\CEDR_I_71_06_P\cats.xml");

            DataTable dt = ds.Tables["content"];
            int j=0;
            foreach (string s in add_list)
            {
                var contents = from content in dt.AsEnumerable()
                               where content.Field<string>("idnumber") == s
                               select new
                               {
                                   firstpage = content.Field<string>("firstpage"),
                                   lastpage = content.Field<string>("lastpage"),
                                   numtypesetpages = content.Field<string>("numtypesetpages")
                 };
                foreach (var item in contents)
                {

                        for ( j=j ; j < Pagedetails.Count; j++)
                        {
                        
                        Console.WriteLine(Files[j]);
                        if (item.firstpage == Pagedetails[j].firstpage && item.lastpage == Pagedetails[j].LastPage && item.numtypesetpages == Pagedetails[j].Pagecount)
                            {
                                Console.WriteLine("page Details Matched");
                                j = j + 1;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("page Details Not Matched");
                            j = j + 1;
                            break;
                        }
                        }                                                            
                }               
            }
            Console.ReadLine();
        }
    }
}
