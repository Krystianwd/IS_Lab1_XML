using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IS_Lab1_XML
{
    internal class XMLReadWithSAXApproach
    {
        internal static void Read(string filepath)
        {
            // konfiguracja początkowa dla XmlReadera
            XmlReaderSettings settings = new XmlReaderSettings();
            var postaci = new Dictionary<string, HashSet<string>>();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
 
            // odczyt zawartości dokumentu
            XmlReader reader = XmlReader.Create(filepath, settings);
            // zmienne pomocnicze
            int count = 0;
            string postac = "";
            string sc = "";
            reader.MoveToContent();
            var Substancje_czynne  = new Dictionary<string, int>();
            var ewidencjaOpakowan = new Dictionary<string, int>();
            string nazwa = "";
            string substancja;
            string element = "";
            // analiza każdego z węzłów dokumentu
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
                {
                    postac = reader.GetAttribute("postac");
                    sc = reader.GetAttribute("nazwaPowszechnieStosowana");
                    nazwa = reader.GetAttribute("nazwaProduktu");
                    //Console.WriteLine(sc);
                    if (postac == "Krem" && sc == "Mometasoni furoas")
                        count++;
                    if (postaci.ContainsKey(sc))
                    {
                        HashSet<string> set = postaci[sc];
                        set.Add(postac);
                        postaci[sc] = set;
                    }
                    else
                    {
                        HashSet<string> set = new HashSet<string>();
                        set.Add(postac);
                        postaci[sc] = set;
                    }
                    if (!ewidencjaOpakowan.ContainsKey(nazwa))
                    {
                        ewidencjaOpakowan.Add(nazwa, 0);
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "substancjeCzynne")
                {
                    element = reader.Name;
                }
                if (reader.NodeType == XmlNodeType.Text && element == "substancjeCzynne")
                {
                    substancja = reader.Value;
                    if (Substancje_czynne.ContainsKey(substancja))
                    {
                        Substancje_czynne[substancja] += 1;
                    }
                    else
                    {
                        Substancje_czynne.Add(substancja, 1);
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "opakowanie")
                {
                    ewidencjaOpakowan[nazwa]++;
                }
            }
            var lista = ewidencjaOpakowan.ToList();
            lista.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            Console.WriteLine("Produkty lecznicze z najwieksza iloscia roznych opakowan:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Nazwa: " + lista[i].Key + ", ilosc roznych opakowan: " + lista[i].Value);
            }
            Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0} ", count);
            int wielopostaciowe = 0;
            foreach (KeyValuePair<string, HashSet<string>> entry in postaci)
            {
                if (entry.Value.Count > 1)
                {
                    wielopostaciowe++;
                }
            }
            Console.WriteLine("Preparaty pod różnymi postaciami: {0}", wielopostaciowe);
            int Max_subancji = 0;
            string Substancja_max = "";
            foreach (KeyValuePair<string, int> entry in Substancje_czynne)
            {
                //Console.WriteLine(entry.Key);
                if (entry.Value > Max_subancji)
                {
                    Max_subancji = entry.Value;
                    Substancja_max = entry.Key;
                }
            }
            Console.WriteLine("substancja czynna występuje w największej liczbie produktów leczniczych: {0}", Substancja_max);
        }
    }
}
