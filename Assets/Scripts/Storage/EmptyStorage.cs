public class EmptyStorage : ASaveLoadJsonTo
{
    public override bool IsValid => true;

    public override bool Initialize(string key) => false;
    public override Return<T> Load<T>(string key) => Return<T>.Empty;
    public override bool Save(string key, object data) => false;
}
