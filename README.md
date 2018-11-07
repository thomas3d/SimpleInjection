# SimpleInjection
replace a C# method in a proprietary dll with your own implementation. 

Example how to use this:

            MethodInfo originalMethod = typeof(NamespaceInproprietaryCode.TransactionData).GetMethod("GetLevel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo injectMethod = typeof(MyClassWithInjectsCode).GetMethod("GetLevel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
             RuntimeReplaceCode.Inject(originalMethod, injectMethod);
            

