using System;

public static class TypeExtension
{
    public static string LastPartOfTypeName(this Type type)
    {
        string[] typeNameFull = type.FullName?.Split('.');
        string typeNameLast = typeNameFull?[typeNameFull.Length - 1];
        return typeNameLast;
    }
}