using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class Bald
    {
        //This class will create a bald and will give us some random, ordered from high to least, numbers
        private static readonly Random t = new Random();//This should be a class member beacuse Random class will make random array only once
        public int[] bald(int n)
        {
            //Making an emply local array to store random numbers
            int[] bald_resault = new int[n];

            //Giving numbers to our local array
            for (int i = 0; i < n; i++)
            {
                bald_resault.SetValue(t.Next(1, 7), i);
            }
            Array.Sort(bald_resault);

            //We should reverce the result beacus normally, it will be sorted from leadt to high
            Array.Reverse(bald_resault);

            return bald_resault;
        }
    }
