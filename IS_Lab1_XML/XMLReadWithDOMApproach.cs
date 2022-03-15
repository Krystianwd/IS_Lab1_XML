using System;
using System.Collections.Generic;
using System.Xml;

namespace IS_Lab1_XML_KD
{
    internal class XMLReadWithDOMApproach
    {
        internal static void Read(string xmlpath)
        {
            {
                XmlDocument doc = new XmlDocument();
                var postaci = new Dictionary<string, HashSet<string>>();
                var Substancje_czynne = new Dictionary<string,int>();
                var Substancje_czynne_doc = doc.GetElementsByTagName("substancjaCzynna");
                doc.Load(xmlpath);
                string postac;
                string sc;
                int count = 0;
                int wartosc = 0;
                string substancja;
                string nazwa = "";
                var drugs = doc.GetElementsByTagName("produktLeczniczy");
                var warianty = new Dictionary<string, HashSet<string>>();
                var substancje = new Dictionary<string, int>();
                var ewidencjaOpakowan = new Dictionary<string, int>();
                foreach (XmlNode d in drugs)
                {
                    postac = d.Attributes.GetNamedItem("postac").Value;
                    sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
                    nazwa = d.Attributes.GetNamedItem("nazwaProduktu").Value;
                    var sklad = d.FirstChild;
                    var opakowania = d.LastChild;
                    if (postac == "Krem" && sc == "Mometasoni furoas") {
                    count++;
                }
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
                    if (ewidencjaOpakowan.ContainsKey(nazwa))
                    {
                        ewidencjaOpakowan[nazwa] += opakowania.ChildNodes.Count;
                    }
                    else
                    {
                        ewidencjaOpakowan.Add(nazwa, opakowania.ChildNodes.Count);
                    }
                }
                for (int i = 0; i < Substancje_czynne_doc.Count; i++)
                {
                    if (Substancje_czynne.ContainsKey(Substancje_czynne_doc[i].InnerXml))
                    {
                        Substancje_czynne[Substancje_czynne_doc[i].InnerXml]++;
                    }
                    else
                    {
                        Substancje_czynne.Add(Substancje_czynne_doc[i].InnerXml, 1);
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
                    if (entry.Value.Count>1)
                    {
                        wielopostaciowe++;
                    }
                }
                Console.WriteLine("Preparaty pod różnymi postaciami: {0}",wielopostaciowe);
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
}