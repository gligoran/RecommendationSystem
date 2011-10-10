using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Models;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.DataAnalysis
{
    public static class Program
    {
        private static TextWriter writer;
        public static void Main()
        {
            InitializeWriter(@"d:\dataset\da\2d-svd.tsv");
            List<string> artistIndexLookupTable;
            var artists = ArtistProvider.Load(DataFiles.Artists, out artistIndexLookupTable);
            var ratings = RatingProvider.Load(DataFiles.EqualFerquencyFiveScaleRatings);

            var ratingsByArtists = new int[artists.Count];
            var artistCount = new int[artists.Count];
            foreach (var rating in ratings)
            {
                ratingsByArtists[rating.ArtistIndex] += (int)rating.Value;
                artistCount[rating.ArtistIndex]++;
            }

            //for (var i = 0; i < ratingsByArtists.Length; i++)
            //    Write(string.Format("{0}\t{1}\t{2}\t{3}", artists[i].Name, ratingsByArtists[i], artistCount[i], (float)ratingsByArtists[i] / (float)artistCount[i]), false);

            //var dict = new Dictionary<string, int>();
            //for (var i = 0; i < ratingsByArtists.Length; i++)
            //    dict.Add(artists[i].Name, ratingsByArtists[i]);

            //var dict = new Dictionary<string, int>();
            //for (var i = 0; i < ratingsByArtists.Length; i++)
            //    dict.Add(artists[i].Name, artistCount[i]);

            //var top = dict./*Where(a => a.Value > 5000).*/OrderByDescending(a => a.Value).Select(a => a.Key).ToArray();

            //var top = new[] { "radiohead", "the beatles", "coldplay", "red hot chili peppers", "muse", "metallica", "pink floyd", "the killers", "linkin park", "nirvana", "system of a down", "queen", "u2", "daft punk", "the cure", "led zeppelin", "placebo", "depeche mode", "david bowie", "bob dylan", "death cab for cutie", "arctic monkeys", "foo fighters", "air", "the rolling stones", "nine inch nails", "sigur rĂłs", "green day", "massive attack", "moby", "amy winehouse", "portishead", "rammstein", "bjĂ¶rk", "kanye west", "bloc party", "johnny cash", "kings of leon", "the white stripes", "beck", "the doors", "oasis", "ac/dc", "the prodigy", "madonna", "the smashing pumpkins", "iron maiden", "jack johnson", "franz ferdinand", "michael jackson", "nightwish", "blink-182", "the offspring", "gorillaz", "incubus", "r.e.m.", "the smiths", "belle and sebastian", "koĐŻn", "feist", "the strokes", "britney spears", "modest mouse", "tool", "interpol", "snow patrol", "pearl jam", "evanescence", "fall out boy", "queens of the stone age", "sufjan stevens", "rĂ¶yksopp", "pixies", "tom waits", "rage against the machine", "the kooks", "bob marley", "in flames", "marilyn manson", "arcade fire", "the chemical brothers", "rihanna", "the clash", "joy division", "mgmt", "eminem", "the shins", "beastie boys", "slipknot", "[unknown]", "boards of canada", "elliott smith", "avril lavigne", "paramore", "norah jones", "beirut", "cat power", "jimi hendrix", "black sabbath", "aphex twin", "disturbed", "the who", "my chemical romance", "regina spektor", "ramones", "nickelback", "elvis presley", "blur", "thievery corporation", "sonic youth", "jamiroquai", "lily allen", "keane", "bob marley & the wailers", "justice", "bright eyes", "nelly furtado", "aerosmith", "a perfect circle", "frank sinatra", "lady gaga", "rise against", "justin timberlake", "manu chao", "goldfrapp", "miles davis", "iron & wine", "maroon 5", "dream theater", "apocalyptica", "kaiser chiefs", "the knife", "the cranberries", "bruce springsteen", "katy perry", "mogwai", "beyoncĂ©", "the beach boys", "neil young", "opeth", "the velvet underground", "tenacious d", "slayer", "sum 41", "tori amos", "children of bodom", "pj harvey", "damien rice", "jimmy eat world", "simon & garfunkel", "megadeth", "animal collective", "john mayer", "guns n' roses", "3 doors down", "30 seconds to mars", "deftones", "dire straits", "enya", "the decemberists", "jason mraz", "limp bizkit", "the postal service", "abba", "alanis morissette", "m.i.a.", "black eyed peas", "deep purple", "audioslave", "eric clapton", "creedence clearwater revival", "bonobo", "hot chip", "morcheeba", "the kinks", "afi", "bon jovi", "pendulum", "billy talent", "jay-z", "mika", "guns n roses", "nina simone", "of montreal", "pantera", "porcupine tree", "papa roach", "robbie williams", "fatboy slim", "bad religion", "dido", "kings of convenience", "christina aguilera", "broken social scene", "james blunt", "bullet for my valentine", "morrissey", "travis", "him", "explosions in the sky", "ladytron", "judas priest", "josĂ© gonzĂˇlez", "nick drake", "tegan and sara", "sonata arctica", "panic at the disco", "crystal castles", "alice in chains", "nouvelle vague", "alicia keys", "the police", "nofx", "misfits", "the flaming lips", "dj shadow", "sublime", "the mars volta", "kylie minogue", "motĂ¶rhead", "pink", "nas", "leonard cohen", "new order", "vampire weekend", "eels", "blind guardian", "the national", "good charlotte", "andrew bird", "timbaland", "garbage", "kelly clarkson", "infected mushroom", "enigma", "fleet foxes", "breaking benjamin", "faithless", "cake", "devendra banhart", "stevie wonder", "tv on the radio", "cocorosie", "the fray", "editors", "2pac", "amon tobin", "the cardigans", "50 cent", "the hives", "imogen heap", "elton john", "johann sebastian bach", "amon amarth", "scorpions", "spoon", "m83", "sting", "groove armada", "killswitch engage", "gnarls barkley", "nick cave and the bad seeds", "soundtrack", "hans zimmer", "atb", "the roots", "the all-american rejects", "burial", "jeff buckley", "dropkick murphys", "brand new", "the ting tings", "the cinematic orchestra", "mariah carey", "snoop dogg", "klaxons", "talking heads", "die Ă„rzte", "three days grace", "lamb", "anathema", "arch enemy", "faith no more", "outkast", "akon", "duffy", "david guetta", "lacuna coil", "cypress hill", "neutral milk hotel", "kasabian", "kate nash", "frank zappa", "architecture in helsinki", "tiĂ«sto", "kiss", "the used", "lenny kravitz", "cut copy", "fleetwood mac", "ratatat", "ozzy osbourne", "stars", "band of horses", "prince", "the libertines", "a tribe called quest", "bon iver", "flogging molly", "billie holiday", "sepultura", "the fratellis", "yann tiersen", "lostprophets", "taking back sunday", "the magnetic fields", "t.i.", "the pussycat dolls", "armin van buuren", "genesis", "dave matthews band", "dashboard confessional", "katatonia", "mando diao", "antony and the johnsons", "cansei de ser sexy", "john lennon", "simple plan", "counting crows", "dimmu borgir", "common", "bloodhound gang", "ludwig van beethoven", "metric", "cradle of filth", "digitalism", "avenged sevenfold", "pavement", "lcd soundsystem", "kraftwerk", "shakira", "cascada", "moloko", "maxĂŻmo park", "no doubt", "rilo kiley", "stereophonics", "rjd2", "katie melua", "pet shop boys", "the verve", "ella fitzgerald", "mindless self indulgence", "unkle", "helloween", "dark tranquillity", "michael bublĂ©", "rancid", "as i lay dying", "guano apes", "sia", "goo goo dolls", "alkaline trio", "jethro tull", "blonde redhead", "rush", "lamb of god", "kt tunstall", "ryan adams", "amy macdonald", "kate bush", "gotan project", "girl talk", "weezer", "ben harper", "tricky", "the jimi hendrix experience", "hooverphonic", "ne-yo", "fiona apple", "howard shore", "lupe fiasco", "yeah yeah yeahs", "brian eno", "razorlight", "rufus wainwright", "van halen", "ray charles", "my bloody valentine", "gogol bordello", "eagles", "john williams", "leona lewis", "the streets", "erykah badu", "lifehouse", "chris brown", "soundgarden", "babyshambles", "sarah mclachlan", "gwen stefani", "basement jaxx", "bryan adams", "coheed and cambria", "james brown", "dead can dance", "atreyu", "notorious b.i.g.", "santana", "scooter", "underworld", "god is an astronaut", "louis armstrong", "ennio morricone", "underoath", "dr. dre", "calexico", "van morrison", "manowar", "adele", "marvin gaye", "ben folds", "john coltrane", "iced earth", "t.a.t.u.", "clint mansell", "ensiferum", "john legend", "kent", "billy joel", "static-x", "new found glory", "phil collins", "autechre", "diana krall", "serj tankian", "phoenix", "blondie", "santogold", "kamelot", "alexisonfire", "nightmares on wax", "soilwork", "manic street preachers", "staind", "amorphis", "the raconteurs", "alice cooper", "godsmack", "korpiklaani", "dragonforce", "mew", "peter gabriel", "paul van dyk", "lil wayne", "cat stevens", "p.o.d.", "janis joplin", "dead kennedys", "behemoth", "the black keys", "mĂşm", "the game", "the dresden dolls", "aretha franklin", "millencolin", "frĂ©dĂ©ric chopin", "de-phazz", "rob zombie", "the new pornographers", "therion", "roxette", "hoobastank", "the mountain goats", "finntroll", "scissor sisters", "atmosphere", "anti-flag", "trivium", "basshunter", "jefferson airplane", "death", "hammerfall", "pulp", "black rebel motorcycle club", "thom yorke", "sade", "patrick wolf", "joe satriani", "anberlin", "cannibal corpse", "stone sour", "lou reed", "camera obscura", "mastodon", "tĂ©lĂ©popmusik", "thrice", "65daysofstatic", "cĂ©line dion", "seether", "clap your hands say yeah", "epica", "duran duran", "jamie cullum", "king crimson", "against me!", "paradise lost", "eddie vedder", "electric light orchestra", "a-ha", "fugazi", "at the drive-in", "lynyrd skynyrd", "enrique iglesias", "squarepusher", "siouxsie and the banshees", "mĂ¶tley crĂĽe", "backstreet boys", "mike oldfield", "the crystal method", "machine head", "jurassic 5", "vangelis", "stone temple pilots", "gym class heroes", "reel big fish", "okkervil river", "usher", "various artists", "booka shade", "wilco", "joanna newsom", "bat for lashes", "rĂłisĂ­n murphy", "lykke li", "funeral for a friend", "enter shikari", "antonio vivaldi", "trentemĂ¸ller", "the dandy warhols", "four tet", "cocteau twins", "the kills", "n*e*r*d", "matisyahu", "stereolab", "nada surf", "minus the bear", "ska-p", "die toten hosen", "stratovarius", "cream", "jens lekman", "the sounds", "the jesus and mary chain", "fergie", "james morrison", "kreator", "dredg", "danny elfman", "ayreon", "joni mitchell" };

            //var topIndices = new int[top.Length];
            //for (var i = 0; i < topIndices.Length; i++)
            //    topIndices[i] = artistIndexLookupTable.IndexOf(top[i]);

            var loader = new ModelLoader<ISvdModel>();
            loader.ModelPartLoaders.Add(new SvdModelPartLoader());
            ISvdModel model = new SvdModel();
            loader.LoadModel(model, @"D:\dataset\models\model.rs");

            //for (var i = 0; i < topIndices.Length; i++)
            //    Write(string.Format("{0}\t{1}\t{2}", top[i], model.ArtistFeatures[0, topIndices[i]], model.ArtistFeatures[1, topIndices[i]]));

            for (var i = 0; i < artistCount.Length; i++)
            {
                if (model.ArtistFeatures[0, i] != 0.1f && model.ArtistFeatures[1, i] != 0.1f)
                Write(string.Format("{0}\t{1}\t{2}\t{3}", artists[i].Name, artistCount[i], model.ArtistFeatures[0, i], model.ArtistFeatures[1, i]), false);
            }

            //var ratingsByArtists = new int[artists.Count];
            //var artistCount = new int[artists.Count];
            //foreach (var rating in ratings)
            //{
            //    ratingsByArtists[rating.ArtistIndex] += (int)rating.Value;
            //    artistCount[rating.ArtistIndex]++;
            //}

            //for (var i = 0; i < ratingsByArtists.Length; i++)
            //{
            //    Write(string.Format("{0}\t{1}\t{2}\t{3}", artists[i].Name, ratingsByArtists[i], artistCount[i], (float)ratingsByArtists[i] / (float)artistCount[i]), false);
            //}

            //foreach (var c in dict.OrderByDescending(d => d.Value))
            //    Write(string.Format("{0}\t{1}", c.Key, c.Value), false);

            /*InitializeWriter(@"d:\dataset\da\users-by-ratings.tsv");

            var users = UserProvider.Load(DataFiles.Users);
            //var ratings = RatingProvider.Load(DataFiles.Playcounts);

            var ratingsByUsers = new int[users.Count];
            foreach (var rating in ratings)
                ratingsByUsers[rating.UserIndex] += (int)rating.Value;

             dict = new Dictionary<string, int>();
            for (var i = 0; i < ratingsByUsers.Length; i++)
                dict.Add(users[i].UserId, ratingsByUsers[i]);

            foreach (var c in dict.OrderByDescending(d => d.Value))
                Write(string.Format("{0}\t{1}", c.Key, Math.Log10(c.Value)), false);*/

            //Console.WriteLine("All");
            //RatingHistogram(DataFiles.EqualFerquencyFiveScaleRatings);

            //Console.WriteLine("Train");
            //RatingHistogram(DataFiles.TrainEqualFerquencyFiveScaleRatings);

            //Console.WriteLine("Test");
            //RatingHistogram(DataFiles.TestEqualFerquencyFiveScaleRatings);

            Console.WriteLine("DONE!");
            Console.ReadLine();
        }

        private static void RatingHistogram(string ratingsFile)
        {
            var ratings = RatingProvider.Load(ratingsFile);

            var ratingBins = new int[5];
            foreach (var rating in ratings)
                ratingBins[((int)rating.Value) - 1]++;

            for (var i = 0; i < ratingBins.Length; i++)
                Console.WriteLine("{0}\t{1}", i + 1, ratingBins[i]);
        }

        #region Writer
        private static void InitializeWriter(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            writer = new StreamWriter(filename);
        }

        private static void Write(string text, bool toConsole = true, bool toFile = true)
        {
            if (toConsole)
                Console.WriteLine(text);

            if (!toFile)
                return;

            writer.WriteLine(text);
            writer.Flush();
        }
        #endregion
    }
}
