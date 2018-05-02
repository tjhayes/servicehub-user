namespace Person.Library.Interfaces
{
    public interface IFactory<TModel> where TModel : IModel
    {
        TModel Make();
    }
}
