namespace Core.Utilities.Uri
{
    using Core.Entities.Concrete;

    public interface IUriService
    {
        System.Uri GeneratePageRequestUri(PaginationFilter filter, string route);
    }
}