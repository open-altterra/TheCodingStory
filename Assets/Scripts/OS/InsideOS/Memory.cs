using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Memory
    {
        public static void SaveData(string path, object data)
        {
            ConsoleController.Print($"Data \"{data}\" has been successfully saved to memory location \"{path}\"!");
        }

        public static bool CheckHash(string id, object data)
        {
            return CodeEditorController.Instance.CheckResult(id, data);
        }
    }
}