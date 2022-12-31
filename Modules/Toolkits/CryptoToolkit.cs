using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Toolkits
{
    public class CryptoToolkit
    {
        public static IEnumerable<byte> Remove(ReadOnlySpan<byte> data)
        {
           IEnumerable<byte> data2 = new List<byte>();

           if (data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                var v = data[3..];

                foreach (var i in v)
                {
                    data2.Append(i);
                }
                return data2;
            }

            foreach (var i in data)
            {
                data2.Append(i);
            }

            return data2;
        }

        public static ReadOnlySpan<byte> Remove(ReadOnlySpan<byte> data,int i =2)
        {
            if (data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                return data[3..];
            }
            return data;
        }

    }
}
