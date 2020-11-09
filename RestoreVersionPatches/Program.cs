using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace RestoreVersionPatches
{
    class Program
    {

        /// <summary>
        /// Загрузка из ресурсов текстовых файлов - чтобы скрипты были внутри программы
        /// </summary>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static string GetSQLFromResource(string ResName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = ResName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }


        }
        static void Main(string[] args)
        {
            if (!File.Exists("version.txt"))
            {
                Console.WriteLine("Нет файла со списком версий");
                Console.ReadKey();
                return;
            }

            string[] version = File.ReadAllLines("version.txt");
            List<PatchEF> patches = new List<PatchEF>();

            PatchEF patch;
            foreach (string p in version)
            {
                if (p.Length<5)
                {
                    continue;
                }

                patch = new PatchEF();
                patch.dt = p.Split(';')[1];
                patch.version = p.Split(';')[0];

                patches.Add(patch);
            }

            string updates = @"c:\temp\updates2\";

            if (args.Length>0)
            {
                if (Directory.Exists(args[0]))
                    updates = args[0].ToString();
            }
            else
            {
                Console.WriteLine("Нужно запустить программу передав ссылку на updates2 где лежат обновления");
                return;
            }

            string directoryName = Path.GetDirectoryName(updates);
            

            foreach (string Name in Directory.EnumerateFiles(directoryName, "*.*", SearchOption.AllDirectories))
            {
                if (!Name.Contains("version.txt"))
                {
                    continue;
                }
                
                foreach (PatchEF p in patches)
                {
                    if (Name.Contains(p.version))
                    {
                        Console.WriteLine("Похоже нашли что нужно");
                        Console.WriteLine(Name);

                        string vtxt = File.ReadAllText(Name);

                        string vtxtresult = vtxt.Split(';')[0]+";"+p.dt;
                        File.WriteAllText(Name, vtxtresult);
                    }
                }
                
                //Console.WriteLine(Name);
            
            }

            Console.ReadKey();
        }
    }
}
