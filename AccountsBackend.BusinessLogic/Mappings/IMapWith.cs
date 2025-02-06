using AutoMapper;

namespace AccountsBackend.BusinessLogic.Mappings
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}


