using AutoMapper;

namespace PortableLibrary.Core.Automapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration GetConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LibraryProfile>();
            });
            return config;
        }
    }
}