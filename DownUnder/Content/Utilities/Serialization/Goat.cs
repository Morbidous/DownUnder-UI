﻿// Sections of this code are autogenerated, changes made to sections 
// between /*GeneratedCodeStart*/ and /*GeneratedCodeEnd*/ will
// not be preserved if the object is editted in DownUnder.UIEditor.

using DownUnder.Content.Utilities;
using System.Collections.Generic;
using DownUnder.Content.UI;
using System;

namespace DownUnder.Content.Utilities
{
    public class Goat : Animal
    {
        public Goat() : base()
        {
            species = "Goat";
            height = 20;
            scale = 0.5f;
            pet_rocks = new List<Rock>()
            {
                new Rock()
                {
                size = 5f,
                bump_count = 2,
    
                }
                ,new Rock()
                {
                size = 3.332f,
                bump_count = 3,

                }
            };
            the_array = new Int32[] { 6, 8, 7 };
            
        }

        public Goat(System.String name) : base(name)
        {
            species = "Goat";
            height = 20;
            scale = 0.5f;
            pet_rocks = new List<Rock>(){new Rock()
{
size = 5f,
bump_count = 2,

}
,new Rock()
{
size = 3.332f,
bump_count = 3,

}
};
            the_array = new Int32[] { 6, 8, 7 };

        }

        public Goat(System.String name, System.Int32 height) : base(name, height)
        {
            species = "Goat";
            height = 20;
            scale = 0.5f;
            pet_rocks = new List<Rock>(){new Rock()
{
size = 5f,
bump_count = 2,

}
,new Rock()
{
size = 3.332f,
bump_count = 3,

}
};
            the_array = new Int32[] { 6, 8, 7 };

        }
    }
}

