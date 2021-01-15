using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Aurora.Utils
{
    static class AssemblyLoader
    {
        public static Assembly LoadFile(string path)
        {
            try
            {
                var domain = AppDomain.CurrentDomain;
                domain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                var assemblyName = AssemblyName.GetAssemblyName(path);
                var assembly = AppDomain.CurrentDomain.Load(assemblyName);
                return assembly;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var searchDir = Path.GetDirectoryName(args.RequestingAssembly.Location);
            foreach (var file in Directory.GetFiles(searchDir, "*.dll"))
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                if (args.Name.Equals(assemblyName.Name))
                {
                    return AppDomain.CurrentDomain.Load(assemblyName);
                }
            }
            return null;
        }
    }
}
