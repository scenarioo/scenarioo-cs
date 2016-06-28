namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Marker interface if the object which implements this interface
    /// has an Id that needs to be sani
    /// </summary>
    public interface ISanitized
    {
        string Id { get; set; } 
        string Name { get; set; }
    }
}