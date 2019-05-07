using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Console_Playground.ValueConverters
{
    public class CurrencyFormatter : IValueConverter<decimal, string>
    {
        public string Convert(decimal sourceMember, ResolutionContext context)
        {
            return sourceMember.ToString("c");
        }
    }
}
