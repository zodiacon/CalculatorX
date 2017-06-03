using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using XCalc.ViewModels;

namespace XCalc {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>(8);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LoadAssemblies() {
            var appAssembly = typeof(App).Assembly;
            foreach (var resourceName in appAssembly.GetManifestResourceNames()) {
                if (resourceName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase)) {
                    using (var stream = appAssembly.GetManifestResourceStream(resourceName)) {
                        var assemblyData = new byte[(int)stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        var assembly = Assembly.Load(assemblyData);
                        _assemblies.Add(assembly.GetName().Name, assembly);
                    }
                }
            }
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        Assembly OnAssemblyResolve(object sender, ResolveEventArgs args) {
            var shortName = new AssemblyName(args.Name).Name;
            if (_assemblies.TryGetValue(shortName, out var assembly)) {
                return assembly;
            }
            return null;
        }

        public App() {
            LoadAssemblies();
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var vm = new MainViewModel();
            var win = new MainWindow { DataContext = vm };
            win.Show();
        }
    }
}
