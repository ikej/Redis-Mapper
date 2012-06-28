using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Youle.Mobile.Core.Helpers
{
    public class StringHelper
    {
        public static string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(1);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
