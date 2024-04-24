

public static class ExtensionsMisc 
{
    public static string ToStringTime(this int self) => $"{(self / 60)}:{self % 60:D2}";


}
