namespace ScenariooTest
{
    using System.Collections.Generic;

    public class ObjectTree
    {
        List<IObjectTree>  list = new List<IObjectTree>();
        List<List<IObjectTree>> list2 = new List<List<IObjectTree>>();

    }

    public interface IObjectTree
    {
        
    }
}
