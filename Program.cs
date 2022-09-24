using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChaseDictionaries
{
    class Program
    {
        static void Main(string[] args)
        {
            var artists = new Dictionary<string, Artist>();
            var artistsByProducer = new Dictionary<string, List<Artist>>();

            //tab delimited   ID  Producer  ArtistName  - no header row
            using (var musicStream = new StreamReader("../../../Music.txt"))
            {
                string ln = musicStream.ReadLine();
                while (ln != null)
                {
                    var parts = ln.Split("\t");
                    if (parts.Length != 3) throw new InvalidDataException("input file has invalid record ");
                    var artist = new Artist(parts[0], parts[1], parts[2]);

                    if (artists.ContainsKey(artist.Id)) throw new InvalidDataException("This input file contains more than one artist with the same ID");
                    artists.Add(artist.Id, artist);

                    if (artistsByProducer.ContainsKey(artist.Producer))
                        //if there is aleady a list of artists for a producer - add it to the existing List
                        artistsByProducer[artist.Producer].Add(artist);
                    else
                        //else create a new dictionary entry for the producer
                        artistsByProducer.Add(artist.Producer, new List<Artist> { artist });


                    ln = musicStream.ReadLine();

                }

                Console.WriteLine($"The artist with ID of A is: {artists["A"]}");
                var cursor = "Enter a name of a producer you would like to seach for (? to see a list of producers):";
                Console.Write(cursor);
                string producerName = null;
                while ((producerName = Console.ReadLine()) != null)
                {
                    if (producerName == "?")
                    {
                        var prdList = "";
                        foreach (var prd in artistsByProducer.Keys) prdList += (prd + "\t");
                        Console.WriteLine(prdList);
                    }
                    else
                    {
                        if (artistsByProducer.ContainsKey(producerName))
                        {
                            Console.WriteLine($"{producerName} produces the following artists:");
                            foreach (var artist in artistsByProducer[producerName])
                                Console.WriteLine($"\t{artist}");
                        }
                        else Console.WriteLine($"{producerName} not found");
                    }
                    Console.Write(cursor);
                }

                
            }


        }


        record Artist(string Id, string Producer, string ArtistName) { }
    }
}
