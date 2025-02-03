using AutoMapper;

namespace AccountsBackend.BusinesLogic.Mapping;

public interface IMapWith<T>
{
    void Mapping(Profile profile) =>
        profile.CreateMap(typeof(T), GetType());
}
