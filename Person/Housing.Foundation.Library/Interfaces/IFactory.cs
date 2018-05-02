namespace Housing.Foundation.Library.Interfaces
{
    public interface IFactory<TModel> where TModel : IModel
    {
        TModel Make();
    }
}
