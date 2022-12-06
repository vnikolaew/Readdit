using AutoMapper;

namespace Readdit.Services.Mapping;

public interface IHaveCustomMappings
{
    void CreateMappings(IProfileExpression configuration);
}