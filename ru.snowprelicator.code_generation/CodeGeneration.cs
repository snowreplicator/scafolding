using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ru.snowprelicator.code_generation
{
    public class CodeGeneration
    {
        public static MemoryStream GenerateCode(ScaffoldedModel scaffoldedModel, bool enableLazyLoading)
        {
            List<string> sourceFiles = PrepareSourceFiles(scaffoldedModel);

            MemoryStream memoryStream = new MemoryStream();
            var result = GenerateCode(sourceFiles, enableLazyLoading).Emit(memoryStream);
            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);
                foreach (var failure in failures)
                {
                    Console.WriteLine(failure.ToString());
                }
                return null;
            }
            return memoryStream;
        }

        private static List<string> PrepareSourceFiles(ScaffoldedModel scaffoldedModel)
        {
            var sourceFiles = new List<string>();
            sourceFiles.Add(scaffoldedModel.ContextFile.Code);
            sourceFiles.AddRange(scaffoldedModel.AdditionalFiles.Select(f => f.Code));
            return sourceFiles;
        }

        private static CSharpCompilation GenerateCode(List<string> sourceFiles, bool enableLazyLoading)
        {
            CSharpParseOptions csharpParseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8);

            IEnumerable<SyntaxTree> parsedSyntaxTrees = sourceFiles.Select(sourceFile => SyntaxFactory.ParseSyntaxTree(sourceFile, csharpParseOptions));

            CSharpCompilation csharpCompilation = CSharpCompilation.Create(
                $"SnowReplicatorDataContext.dll",
                parsedSyntaxTrees,
                references: CompilationReferences(enableLazyLoading),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default)
                );

            return csharpCompilation;
        }
        private static List<MetadataReference> CompilationReferences(bool enableLazyLoading)
        {
            List<MetadataReference> metadataReferences = new List<MetadataReference>();

            AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            IEnumerable<MetadataReference> metadataReferenceList = referencedAssemblies.Select(a => MetadataReference.CreateFromFile(Assembly.Load(a).Location));
            foreach (MetadataReference metadataReference in metadataReferenceList)
            {
                metadataReferences.Add(metadataReference);
            }

            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0").Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(System.Data.Common.DbConnection).Assembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.Expression).Assembly.Location));
            if (enableLazyLoading)
            {
                metadataReferences.Add(MetadataReference.CreateFromFile(typeof(ProxiesExtensions).Assembly.Location));
            }

            return metadataReferences;
        }

    }
}