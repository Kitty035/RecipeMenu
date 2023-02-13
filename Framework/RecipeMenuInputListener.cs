using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace RecipeMenu.Framework
{
    internal class RecipeMenuInputListener : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The Recipe helper.</summary>
        private readonly Recipe Recipe;

        /// <summary>The Machines helper.</summary>
        private readonly Machines Machines;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);
        
        /// <summary>Defines if button is used on Machines tasks rather than daily tasks.</summary>
        private readonly bool IsMachinesButton;

        /// <summary>Area on the screen that the button occupies. Used to check if we click on it.</summary>
        private Rectangle SetButtonBounds;

        /// <summary>The original menu, so it can be refreshed.</summary>
        private readonly RecipeMenu RecipeMenu;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct a button with Recipe helper.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="Recipe">The Recipe helper.</param>
        /// <param name="Recipemenu">The RecipeMenu creating this button.</param>
        public RecipeMenuInputListener(string label, int slotWidth, Recipe Recipe, RecipeMenu Recipemenu)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.Recipe = Recipe;
            this.RecipeMenu = Recipemenu;
        }

        /// <summary>Construct a button with Machines helper.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="Machines">The Machines helper.</param>
        /// <param name="Recipemenu">The RecipeMenu creating this button.</param>
        public RecipeMenuInputListener(string label, int slotWidth, Machines Machines, RecipeMenu Recipemenu)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.Machines = Machines;
            this.RecipeMenu = Recipemenu;
            this.IsMachinesButton = true;
        }

        /// <summary>Called when player left clicks on the menu.</summary>
        /// <param name="x">X coordinate of the click.</param>
        /// <param name="y">Y coordinate of the click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut ||!this.SetButtonBounds.Contains(x, y)) { return; }      // Didn't click on button. Do nothing.
            else if (this.IsMachinesButton) { this.Machines.CompleteTask(label); }    // Clicked on Machines button!
            else { this.Recipe.CompleteTask(label); }                                  // Clicked on Recipe button!

            Game1.activeClickableMenu = new RecipeMenu(RecipeMenu);                   // Refresh the Recipe menu.
            Game1.soundBank.PlayCue("achievement");                                     // Play a sound!
            return;
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            Utility.drawTextWithShadow(
                spriteBatch,
                this.label, 
                Game1.dialogueFont, 
                new Vector2(
                    this.bounds.X + slotX, 
                    this.bounds.Y + slotY), 
                this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 
                1f, 
                0.15f);

            Utility.drawWithShadow(
                spriteBatch, 
                Game1.mouseCursors, 
                new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), 
                this.SetButtonSprite, 
                Color.White, 
                0.0f, 
                Vector2.Zero,
                Game1.pixelZoom,
                false, 
                0.15f
                );
            return;
        }
    }
}