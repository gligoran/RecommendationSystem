using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDotNet;
using System.Diagnostics;

namespace RecommenderSystem.Data.Tester
{
    class Program
    {
        static REngine r;

        static void Main(string[] args)
        {
            REngine.SetDllDirectory(@"C:\Program Files\R\R-2.13.1\bin\i386");
            REngine.CreateInstance("RDotNet");
            r = REngine.GetInstanceFromID("RDotNet");
            Stopwatch timer = new Stopwatch();

            /*long load, save;

            //10k users
            Console.WriteLine("Loading users...");

            timer.Start();
            var users = Manager.LoadUsers("d:/dataset/usersha1-profile-10ku.tsv", r);
            timer.Stop();
            load = timer.ElapsedMilliseconds;

            Console.WriteLine("Users loaded ({0}ms).", load);
            Console.WriteLine("Loading play counts...");

            timer.Reset();
            timer.Start();
            Manager.LoadPlayCounts("d:/dataset/usersha1-artmbid-artname-plays-10ku.tsv", users, r);
            timer.Stop();
            load = timer.ElapsedMilliseconds;

            Console.WriteLine("Play counts loaded ({0}ms).", load);
            Console.WriteLine("Saving to XML...");

            timer.Reset();
            timer.Start();
            Manager.SerializeData(users, @"D:\dataset\10ku.xml");
            timer.Stop();
            save = timer.ElapsedMilliseconds;
            Console.WriteLine("XML saved ({0}ms).", save);
            
            //1240k plays
            Console.WriteLine("Loading users...");

            timer.Start();
            users = Manager.LoadUsers("d:/dataset/usersha1-profile-1240kp.tsv", r);
            timer.Stop();
            load = timer.ElapsedMilliseconds;

            Console.WriteLine("Users loaded ({0}ms).", load);
            Console.WriteLine("Loading play counts...");

            timer.Reset();
            timer.Start();
            Manager.LoadPlayCounts("d:/dataset/usersha1-artmbid-artname-plays-1240kp.tsv", users, r);
            timer.Stop();
            load = timer.ElapsedMilliseconds;

            Console.WriteLine("Play counts loaded ({0}ms).", load);
            Console.WriteLine("Saving to XML...");

            timer.Reset();
            timer.Start();
            Manager.SerializeData(users, @"D:\dataset\1240kp.xml");
            timer.Stop();
            save = timer.ElapsedMilliseconds;
            Console.WriteLine("XML saved ({0}ms).", save);*/

            var users = Manager.DeserializeData<User>(@"D:\dataset\10ku.xml");
            

            Console.ReadLine();
        }
    }
}
