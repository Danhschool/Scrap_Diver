using UnityEngine;

public static class CaesarCipherUtility
{
    private const int SHIFT_KEY = 5;

    public static string Encrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) return data;

        char[] buffer = data.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (char)(buffer[i] + SHIFT_KEY);
        }
        return new string(buffer);
    }

    public static string Decrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) return data;

        char[] buffer = data.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (char)(buffer[i] - SHIFT_KEY);
        }
        return new string(buffer);
    }
}