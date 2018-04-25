/******************************** Module Header ********************************\
Module Name:  RecipeFileDefinition.cs
Project:      CSShellExtThumbnailHandler
Copyright (c) Microsoft Corporation.

The RecipeFileDefinition class defines the key elements in a recipe file.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\*******************************************************************************/

#region Using directives
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion


namespace CSShellExtThumbnailHandler
{
    [ComVisible(false)]
    public class RecipeFileDefinition
    {
        public RecipeFileDefinition(Stream stream)
        {
            // Load the XML document, and extract its properties.
            XDocument doc = XDocument.Load(stream);

            this.Title = doc.Root.Element("Title").Value;
            this.Author = doc.Root.Element("Background").Element("Author").Value;
            this.RecipeKeywords = new List<string>();
            foreach (XElement e in doc.Root.Element("RecipeKeywords").Elements("Keyword"))
            {
                this.RecipeKeywords.Add(e.Value);
            }
            this.Info = new RecipeInfo(doc.Root.Element("RecipeInfo"));
            this.Comments = doc.Root.Element("Comments").Value;
            this.Ingredients = new List<RecipeIngredient>();
            foreach (XElement e in doc.Root.Element("Ingredients").Elements("Item"))
            {
                this.Ingredients.Add(new RecipeIngredient(e));
            }
            this.Directions = new List<string>();
            foreach (XElement e in doc.Root.Element("Directions").Elements("Step"))
            {
                this.Directions.Add(e.Value);
            }

            var pic = doc.Root.Element("Attachments").Element("Picture");
            this.EncodedPicture = pic.Attribute("Source").Value;
        }

        public string Title { get; private set; }
        public string Author { get; private set; }
        public List<string> RecipeKeywords { get; private set; }
        public RecipeInfo Info { get; private set; }
        public string Comments { get; private set; }
        public List<RecipeIngredient> Ingredients { get; private set; }
        public List<string> Directions { get; private set; }
        public string EncodedPicture { get; private set; }
    }

    [ComVisible(false)]
    public class RecipeInfo
    {
        public RecipeInfo(XElement info)
        {
            var diff = info.Element("Difficulty");
            if (diff != null)
            {
                this.Difficulty = diff.Value;
            }
            var prepTime = info.Element("PreparationTime");
            if (prepTime != null)
            {
                this.PreparationTime = int.Parse(prepTime.Value);
            }
            var cookTime = info.Element("CookTime");
            if (cookTime != null)
            {
                this.CookTime = int.Parse(cookTime.Value);
            }
        }

        public string Difficulty { get; private set; }
        public int PreparationTime { get; private set; }
        public int CookTime { get; private set; }
    }

    [ComVisible(false)]
    public class RecipeIngredient
    {
        public RecipeIngredient(XElement ingredient)
        {
            this.Item = ingredient.Value;
            var quantity = ingredient.Attribute("Quantity");
            if (quantity != null)
            {
                this.Quantity = float.Parse(quantity.Value);
            }
            var unit = ingredient.Attribute("Unit");
            if (unit != null)
            {
                this.Unit = unit.Value;
            }
        }

        public string Item { get; private set; }
        public float Quantity { get; private set; }
        public string Unit { get; private set; }
    }
}