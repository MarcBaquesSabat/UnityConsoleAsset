using UnityEngine;

namespace BlackRefactory.Console
{
    public class ConsoleUtilities : MonoBehaviour
    {
        [Command("VariablePublicInt")]
        public int a = 0;
        [Command("VariablePublicStaticInt")]
        public static int b = 0;
        [Command("VariablePrivateInt")]
        private int c = 0;
        [Command("VariablePrivateStaticInt")]
        private static int d = 0;
        [Command("PropiertyPublicInt")]
        public int Caso { get; set; }
    
        [Command("FunctionPublicVoid")]
        public void PublicFunctionTest()
        {
            Debug.Log("Public void Test");
        }
    
        [Command("FunctionPublicStativVoid")]
        public static void PublicStaticTest()
        {
            Debug.Log("Public Static void Test");
        }

        [Command("FunctionPrivateVoid")]
        private void PrivateVoidTest()
        {
            Debug.Log("Private void Command");
        }
    
        [Command("FunctionPrivateStaticVoid")]
        private static void PrivateStaticTest()
        {
            Debug.Log("Private static void command");
        }
    }
}
