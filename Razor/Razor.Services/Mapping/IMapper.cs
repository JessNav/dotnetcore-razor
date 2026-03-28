namespace Razor.Services.Mapping
{
    // Simple generic mapping interface
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source) where TSource : class where TDestination : class, new();
    }
}
