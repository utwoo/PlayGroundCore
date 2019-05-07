using System;
using System.Linq;
using AutoMapper;
using AutoMapper.Mappers;
using Console_Playground.Models.Destinations;
using Console_Playground.Models.Sources;
using Console_Playground.ValueConverters;
using Newtonsoft.Json;

namespace Console_Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Simple, SimpleDTO>());
            var executionPlan = configuration.BuildExecutionPlan(typeof(Simple), typeof(SimpleDTO));

            Console.WriteLine("Successfully.");
            Console.ReadLine();
        }
    }
}
