using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;



public class Cryptor : MonoBehaviour
{
    [SerializeField] private string m_PathInput;
    [SerializeField] private string m_PathToSave;
    [SerializeField] private string m_PathToRestore;
    [SerializeField] private int m_Key;
    [SerializeField] private bool m_IsCripting;
    [SerializeField] private string m_ColorTrue = "green";
    [SerializeField] private string m_ColorFalse = "red";
    
    
    [ContextMenu("ConvertToByte")]
    private async void ConvertToByte()
    {
        byte[] bytes = File.ReadAllBytes(m_PathInput);
        string cryptByte = Convert.ToBase64String(bytes);
        using StreamWriter writer = new StreamWriter(m_PathToSave, false);
        Debug.Log("<color=yellow>Convert to binary</color>");
        await writer.WriteAsync(cryptByte);
    }

    [ContextMenu("Cryption")]
    private async Task<string> Encrypt()
    {
        string text = await Read();
        using StreamWriter writer = new StreamWriter(m_PathToSave, false); 
        writer.WriteAsync(XOREncryptDecrypt(text));
        Debug.Log(!m_IsCripting ? $"<color={m_ColorTrue}>Cryption</color>" : $"<color={m_ColorFalse}>Cryption</color>");
        m_IsCripting = !m_IsCripting;
        return writer.ToString();
    }
    
     
    [ContextMenu("Restore")]
    private async void Decrypt()
    {
        File.WriteAllBytes(m_PathToRestore, Convert.FromBase64String(await Read()));
        Debug.Log("<color=blue>Restore</color>");
    }
    
    
    private string XOREncryptDecrypt(string inputData)
    {
        StringBuilder outSB = new StringBuilder(inputData.Length);
        for (var i = 0; i < inputData.Length; i++)
        {
            var ch = (char) (inputData[i] ^ m_Key);
            outSB.Append(ch);
        }
        return outSB.ToString();
    }
    

    private async Task<string> Read()
    {
        using StreamReader reader = new StreamReader(m_PathToSave);
        var text =  await reader.ReadToEndAsync();
        return text;
    }

    
}
