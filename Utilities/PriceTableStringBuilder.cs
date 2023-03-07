using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PrsUtilities;

public static class PriceTableStringBuilder
{

    public static string CreatePriceTable(List<PricedRequestLine> pricedLines)
    {
        string title =    "--------------------------------Price Table--------------------------------------------------";
        string header =   "\n---------------------------RequestId:{0}---------------------------------------------------\n";
        string header2 =  "\nProductId---------------ProductPrice-------------Quantity---------------SumLineCost---------\n";
        string repeatRow ="\n{0,-20} {1,-20} {2,-25} {3,-25}\n";
        string footer =   "\n---------------------------RequestRotal:{0}--------------------------------------------------\n";


        Debug.WriteLine($"PricedLines size is {pricedLines.Count}");

        StringBuilder builder = new StringBuilder();
        
        builder.Append(string.Format(header, pricedLines[0].RequestId));
        builder.Append(header2);      
        
        
        

        foreach(PricedRequestLine pricedLine in pricedLines)
        {
            builder.Append(string.Format(
                repeatRow, pricedLine.ProductId, decimal.Parse(pricedLine.ProductPrice).ToString("C"), 
                pricedLine.Quantity, decimal.Parse(pricedLine.LineItemPrice).ToString("C")));
           
        }
        
        builder.Append(string.Format(footer, decimal.Parse(pricedLines[0].RequestCost).ToString("C")));
        return builder.ToString();

    }

    
}
