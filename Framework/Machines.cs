using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RecipeMenu.Framework
{
    class Machines
    {
        /// <summary>List of items on the Machines.</summary>
        private readonly List<string> MachinesItems;

        /// <summary>Construct a Machines by reading the Machines.txt file, or create a file if it doesn't exist.</summary>
        public Machines()
        {
            string filename = Path.Combine("Mods", "RecipeMenu", "Plans", "Machines.txt");  // TODO: Make sure we don't need the complete path from the root directory.
            if (File.Exists(filename))  // Read each line of the file into a list, then remove blank entries. 
            {
                this.MachinesItems = new List<string>(File.ReadAllLines(filename, Encoding.UTF8));  
                this.MachinesItems.Remove("");
                this.MachinesItems.Remove(" ");
            } else  // If file doesn't exist, create it and fill it with instructions on how to edit it.
            {
                string[] list = new string[] { "Find RecipeMenu/Plans/Machines.txt.", "Open it in notepad.", "Add your tasks.", "Open this menu agian." };
                File.WriteAllLines(filename, list);
            }
             
            
        }

        public List<string> GetMachinesItems()
        {
            return this.MachinesItems;
        }

        /// <summary>Delete item from the Machines, then save the file.</summary>
        /// <param name="label">Text of the item being marked off.</param>
        public void CompleteTask(string label)
        {
            this.MachinesItems.Remove(label);
            string filename = Path.Combine("Mods", "RecipeMenu", "Plans", "Machines.txt");
            File.WriteAllLines(filename, this.MachinesItems);
        }
    }
}