namespace FilesManager.Application.Helpers
{
    public static class ObjectExtensions
    {
        public static TProperty GetProperty<TEntity, TProperty>(this TEntity entity, string propertyName)
        {
            return (TProperty)typeof(TEntity).GetProperty(propertyName).GetValue(entity);
        }
    }
}
