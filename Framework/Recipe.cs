using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace RecipeMenu.Framework
{
    class RecipeMenu
    {
        private readonly string Filename;
        private StreamReader Reader;
        private List<List<string>> data;
        private List<string> RecipeMenu;

        public RecipeMenu(int year, string filepath)
        {
            this.Filename = year.ToString() + ".csv";
            bool AccessDenied = false;
            try
            {
                this.Reader = new StreamReader(File.OpenRead(Path.Combine(filepath, "Plans", this.Filename)));
            } catch (FileNotFoundException)
            {
                File.Copy(Path.Combine(filepath, "Plans", "Template.csv"), Path.Combine("Mods", "RecipeMenu", "Plans", this.Filename));
                this.Reader = new StreamReader(File.OpenRead(Path.Combine(filepath, "Plans", this.Filename)));
            } catch (IOException)
            {
                this.Reader = new StreamReader(File.OpenRead(Path.Combine(filepath, "Plans", "Template.csv")));
                AccessDenied = true;
            }
            
            this.data = new List<List<string>>();

            while (!this.Reader.EndOfStream)
            {
                var line = this.Reader.ReadLine();
                var values = line.Split(',').ToList();

                while (values.Contains("")) { values.Remove(""); }
                values.RemoveAt(0);

                data.Add(values.ToList());
            }

            Reader.Close();

            if (AccessDenied)
            {
                data[0].Clear();
                data[0].Add("Unable to access plan.");
                data[0].Add("Please close your spreadsheet program.");
                data[0].Add("Then, reload this save.");
            }
        }

        public override string ToString()
        {
            return data[1][0];
        }

        public void CreateRecipeMenu()
        {
            int season = this.SeasonToInt(StardewModdingAPI.Utilities.SDate.Now().Season);
            
            int day = StardewModdingAPI.Utilities.SDate.Now().Day;

            if (day <= 0) { day = 1; }
            if (day >= 29) { day = 28; }

            int SeasonRow = (season - 1) * 29 + 1;
            int DayRow = SeasonRow + day;

            this.RecipeMenu = new List<string>();
            this.RecipeMenu.Clear();
            this.RecipeMenu = data[0];
            this.RecipeMenu.AddRange(data[SeasonRow]);
            this.RecipeMenu.AddRange(data[DayRow]);
        }

        public List<string> GetRecipeMenu()
        {
            return this.RecipeMenu;
        }

        public void CompleteTask(string task)
        {
            this.RecipeMenu.Remove(task);
        }

        private List<string> GetTasksForDay(int season, int day)
        {
            List<string> ReturnList = new List<string>();

            switch (season) {
                case 1:  // Spring
                    ReturnList.Add("Spring");
                    break;
                case 2:  // Summer
                    ReturnList.Add("Summer");
                    break;
                case 3:  //  Fall
                    ReturnList.Add("Fall");
                    break;
                case 4:  //  Winter
                    ReturnList.Add("Winter");
                    break;
                default:  // Season out of bounds
                    return ReturnList;
            }

            ReturnList[0] = ReturnList[0] + " " + day.ToString() + ":";
            int DayRow = (season - 1) * 29 + 1 + day;
            ReturnList.AddRange(data[DayRow]);

            return ReturnList;
        }

        public List<string> CreateWeekList()
        {
            int season = this.SeasonToInt(StardewModdingAPI.Utilities.SDate.Now().Season);
            int day = StardewModdingAPI.Utilities.SDate.Now().Day;

            if (day <= 0) { day = 1; }
            if (day >= 29) { day = 28; }

            List<string> ReturnList = new List<string>();

            for (int i = 1; i <= 7; i++)
            {
                if (season < 5)
                {
                    ReturnList.AddRange(this.GetTasksForDay(season, day));
                    ReturnList.Add(" ");  // Blank line

                    day++;
                    if (day == 29)
                    {
                        day = 1;
                        season++;
                    }
                }
            }

            return ReturnList;
        }

        public List<string> CreateMonthList()
        {
            int season = this.SeasonToInt(StardewModdingAPI.Utilities.SDate.Now().Season);

            List<string> ReturnList = new List<string>();

            for (int day = 1; day <= 28; day++)
            {
                ReturnList.AddRange(this.GetTasksForDay(season, day));
                ReturnList.Add(" ");  // Blank line
            }

            return ReturnList;
        }

        private int SeasonToInt(string season) 
        {
            switch (season.ToLower())
            {
                case "spring":
                    return 1;
                case "summer":
                    return 2;
                case "fall":
                    return 3;
                case "winter":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}