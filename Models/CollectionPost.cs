namespace Backend.Models;

public class CollectionPost {
    public int Id {get; set;}

    public Post? Post {get; set;}

    public int? PostId {get; set;}

    public Collection? Collection {get; set;}

    public int CollectionId {get; set;}
}
