using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MyNamespace
{
    public class RuntimeReplaceCode
    {
        public static void Inject(MethodInfo originalMethod, MethodInfo injectMethod)
        {
            if (!HasSameSignature(originalMethod, injectMethod))
            {
                throw new ArgumentException("originalMethod and injectMethod must have have same signature");
            }

            RuntimeHelpers.PrepareMethod(originalMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(injectMethod.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)injectMethod.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)originalMethod.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)injectMethod.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)originalMethod.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    *tar = *inj;
#endif
                }
            }
        }

        private static bool HasSameSignature(MethodInfo a, MethodInfo b)
        {
            var aParam = a.GetParameters().ToList();
            var bParam = b.GetParameters().ToList();
            if (aParam.Count != bParam.Count)
                return false;

            for (int i = 0; i < aParam.Count; i++)
            {
                if (aParam[i].ParameterType != bParam[i].ParameterType)
                    return false;
                if (aParam[i].Name != bParam[i].Name)  // may not be required. 
                    return false;
            }

            return a.ReturnType == b.ReturnType;
        }
    }
}
