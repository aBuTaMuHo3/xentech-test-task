using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MinLibs.Utils;
using UnityEditor;
using UnityEngine;

namespace MinLibs.MVC
{
	public static class ModularRegistrationsGenerator
	{
		const int INDENT = 2;

		public static void GenerateRegistrations<T>(EnvironmentState environmentState) where T: class, IRegistrations, new()
		{
			var reg = new T();
			var info = reg.Create();
			var time = Time.realtimeSinceStartup;

			FilterRegistrations(info.Registrations, environmentState);
			GenerateRegisterCommand(info);

			var now = Time.realtimeSinceStartup;
			var diff = now - time;
			Debug.Log(info.FilePath + " >>> " + diff);

			AssetDatabase.Refresh();
		}

		private static void FilterRegistrations(IList<RegistrationData> registrations, EnvironmentState state)
		{
			var typeMap = new Dictionary<Type, List<RegistrationData>>();

			foreach (var registration in registrations)
			{
				foreach (var interfaceType in registration.InterfaceTypes)
				{
					var list = typeMap.Retrieve(interfaceType);
					list.Add(registration);
				}
			}

			foreach (var pair in typeMap)
			{
				var targetType = pair.Key;
				var candidates = pair.Value;

				if (candidates.Count > 1)
				{
					RegistrationData bestCandidate = null;
					var maxRating = 0;

					foreach (var candidate in candidates)
					{
						if (candidate.Platforms.Has(state.platform)
							&& candidate.Editor.Has(state.editor)
							&& candidate.Envs.Has(state.env)
							&& candidate.Providers.Has(state.provider))
						{
							var rating = GetRating(state.editor, candidate);
							Debug.Log("Rating for " + candidate.ClassType + ": " + rating);

							if (rating >= maxRating)
							{
								maxRating = rating;
								bestCandidate = candidate;
							}
//							else if (rating == maxRating && bestCandidate != candidate)
//							{
//								throw new Exception(candidate + " and " + bestCandidate + " have the same rating of " + rating);
//							}
						}
					}

					if (bestCandidate == null)
					{
						throw new Exception("no suitable candidate for " + targetType);
					}

					candidates.Remove(bestCandidate);

					foreach (var rejected in candidates)
					{
						registrations.Remove(rejected);
					}
				}
			}
		}

		static int GetRating(EditorFlags editor, RegistrationData candidate)
		{
			var rating = 0;

			if (candidate.Editor == editor)
			{
				rating += 1000;
			}

			if (candidate.Envs < EnvFlags.All)
			{
				rating += 100;
			}

			if (candidate.Providers < ProviderFlags.All)
			{
				rating += 10;
			}

			if (candidate.Platforms < PlatformFlags.All)
			{
				rating += 1;
			}

			return rating;
		}

		static void GenerateRegisterCommand(RegistrationInfo registrationInfo)
		{
			var typeVarMap = new Dictionary<Type, string>();
			var parameterBuilder = new List<string>();
			var declarationBuilder = new List<string>();
			var registrationBuilder = new List<string>();
			var handlerBuilder = new List<string>();
			var assignmentBuilder = new List<string>();
			var cleanupBuilder = new List<string>();
			var postInjectionBuilder = new List<string>();

			foreach (var registrationData in registrationInfo.Registrations)
			{
				var flags = registrationData.Flags;

				if (flags.Has(RegisterFlags.NoCache))
				{
					RegisterTypeByHandler(registrationData, typeVarMap, handlerBuilder);
				}
				else if (flags.Has(RegisterFlags.Parameter))
				{
					var index = parameterBuilder.Count;
					var parameter = "var {0} = ({1}) args[" + index + "];";
					RegisterMultipleTypes(registrationData, typeVarMap, parameterBuilder, registrationBuilder, parameter, false);
				}
				else
				{
					const string DECLARATION = "var {0} = new {1}();";
					RegisterMultipleTypes(registrationData, typeVarMap, declarationBuilder, registrationBuilder, DECLARATION, true);
				}
			}

			AssignInjections(registrationInfo.Registrations, typeVarMap, assignmentBuilder, cleanupBuilder, postInjectionBuilder);

			var parameters = string.Join(Environment.NewLine, parameterBuilder);

			UpdateRegistrationCommand(
				registrationInfo,
				parameters,
				string.Join(Environment.NewLine, declarationBuilder),
				string.Join(Environment.NewLine, registrationBuilder),
				string.Join(Environment.NewLine, handlerBuilder),
				string.Join(Environment.NewLine, assignmentBuilder),
				string.Join(Environment.NewLine, cleanupBuilder),
				string.Join(Environment.NewLine, postInjectionBuilder));
		}

		static void RegisterMultipleTypes(RegistrationData registrationData, IDictionary<Type, string> typeVarMap,
											List<string> declarationBuilder, List<string> registrationBuilder, string declaration, bool preventInjections)
		{
			var type = registrationData.ClassType;
			var varName = CreateVarName(type);
			typeVarMap.Add(type, varName);
			AppendLine(declarationBuilder, string.Format(declaration, varName, type.FullName));

			foreach (var interfaceType in registrationData.InterfaceTypes)
			{
				const string REGISTERINSTANCE_IMPLICIT = "context.RegisterInstance({1}, {2});";
				const string REGISTERINSTANCE_EXPLICIT = "context.RegisterInstance<{0}>({1}, {2});";
				var template = interfaceType == type ? REGISTERINSTANCE_IMPLICIT : REGISTERINSTANCE_EXPLICIT;
				var flags = ParseRegisterFlags(registrationData.Flags, preventInjections);

				AppendLine(registrationBuilder, string.Format(template, interfaceType.FullName, varName, flags));
			}
		}

		static void RegisterTypeByHandler(RegistrationData registrationData, IDictionary<Type, string> typeVarMap,
											List<string> handlerBuilder)
		{
			const string HANDLER_REGISTRATION_0 = "context.RegisterHandler<{0}>(host => {{";
			const string HANDLER_REGISTRATION_1 = "var {0} = new {1}();";
			const string HANDLER_REGISTRATION_2 = "return {0};";
			const string HANDLER_REGISTRATION_3 = "}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);";

			var type = registrationData.ClassType;
			var varName = CreateVarName(type);
			typeVarMap.Add(type, varName);

			foreach (var interfaceType in registrationData.InterfaceTypes)
			{
				AppendLine(handlerBuilder, string.Format(HANDLER_REGISTRATION_0, interfaceType.FullName));
				AppendLine(handlerBuilder, string.Format(HANDLER_REGISTRATION_1, varName, type.FullName), 1);

				var assignmentBuilder = new List<string>();
				var cleanupBuilder = new List<string>();
				var postInjectionBuilder = new List<string>();
				RenderInjectionInfo(type, typeVarMap, assignmentBuilder, cleanupBuilder, postInjectionBuilder, 1);

				AppendLine(handlerBuilder, assignmentBuilder);
				AppendLine(handlerBuilder, cleanupBuilder);
				AppendLine(handlerBuilder, postInjectionBuilder);

				AppendLine(handlerBuilder, string.Format(HANDLER_REGISTRATION_2, varName), 1);
				AppendLine(handlerBuilder, HANDLER_REGISTRATION_3);
				AppendLine(handlerBuilder);
			}
		}

		static void AssignInjections(IList<RegistrationData> registrationDataList, IDictionary<Type, string> typeVarMap,
									List<string> assignmentBuilder, List<string> cleanupBuilder, List<string> postInjectionBuilder)
		{
			foreach (var registrationData in registrationDataList)
			{
				if (!registrationData.Flags.Has(RegisterFlags.NoCache))
				{
					RenderInjectionInfo(registrationData.ClassType, typeVarMap,
						assignmentBuilder, cleanupBuilder, postInjectionBuilder, 0);
				}
			}
		}

		static void RenderInjectionInfo(Type type, IDictionary<Type, string> typeVarMap, List<string> assignmentBuilder,
										List<string> cleanupBuilder,
										List<string> postInjectionBuilder, int indent)
		{
			var instanceName = typeVarMap[type];
			var info = InfoParser.Parse(type, BindingFlags.SetField, InjectorFlags.None);
			RenderInjections(typeVarMap, assignmentBuilder, info, instanceName, indent);
			RenderCleanups(cleanupBuilder, info, instanceName, indent);
			RenderPostInjections(postInjectionBuilder, info, instanceName, indent);
		}

		static void RenderInjections(IDictionary<Type, string> typeVarMap, List<string> assignmentBuilder, InjectionInfo info,
									string instanceName, int indent)
		{
			const string ASSIGNMENT = "{0}.{1} = {2};";
			const string CONTEXT_GET = "context.Get<{0}>()";

			var injections = info.Injections;

			if (injections != null && injections.Count > 0)
			{
				foreach (var injection in injections)
				{
					var injectedType = injection.Value;
					string injectedName;

					if (!typeVarMap.TryGetValue(injectedType, out injectedName))
					{
						injectedName = string.Format(CONTEXT_GET, injectedType.FullName);
					}

					AppendLine(assignmentBuilder, string.Format(ASSIGNMENT, instanceName, injection.Key, injectedName), indent);
				}

				AppendLine(assignmentBuilder);
			}
		}

		static void RenderCleanups(List<string> cleanupBuilder, InjectionInfo info, string instanceName, int indent)
		{
			const string CLEANUP_REGISTRATION = "context.OnCleanUp.AddListener({0}.{1});";

			var cleanups = info.GetCalls<Cleanup>();

			if (cleanups != null && cleanups.Count > 0)
			{
				foreach (var cleanup in cleanups)
				{
					AppendLine(cleanupBuilder, string.Format(CLEANUP_REGISTRATION, instanceName, cleanup.Name), indent);
				}
			}
		}

		static void RenderPostInjections(List<string> postInjectionBuilder, InjectionInfo info, string instanceName, int indent)
		{
			const string POST_INJECTION_CALL = "{0}.{1}();";

			var postInjections = info.GetCalls<PostInjection>();

			if (postInjections != null && postInjections.Count > 0)
			{
				foreach (var postInjection in postInjections)
				{
					AppendLine(postInjectionBuilder, string.Format(POST_INJECTION_CALL, instanceName, postInjection.Name), indent);
				}
			}
		}

		static string CreateVarName(Type type)
		{
			var varName = type.FullName.Replace(".", string.Empty);

			return char.ToLowerInvariant(varName[0]) + varName.Substring(1);
		}

		static void UpdateRegistrationCommand(RegistrationInfo info,
											  string parameters,
											  string declarations,
											  string registrations,
											  string handlers,
											  string assignments,
											  string cleanups,
											  string postInjections,
											  [CallerFilePath] string sourceFilePath = "")
		{
			var filePath = Path.Combine(Application.dataPath, info.FilePath);
			filePath = Path.Combine(filePath, info.ClassName + ".cs");

			var templatePath = Path.GetDirectoryName(sourceFilePath);
			var templateFile = Path.Combine(templatePath, "RegistrationTemplate.txt");

			var text = File.ReadAllText(templateFile);
			text = text.Replace("// NAMESPACE", info.NameSpace);
			text = text.Replace("// CLASSNAME", info.ClassName);
			text = text.Replace("// PARAMETERS", parameters);
			text = text.Replace("// DECLARATIONS", declarations);
			text = text.Replace("// REGISTRATIONS", registrations);
			text = text.Replace("// HANDLERS", handlers);
			text = text.Replace("// ASSIGNMENTS", assignments);
			text = text.Replace("// CLEANUPS", cleanups);
			text = text.Replace("// POSTINJECTIONS", postInjections);
			Debug.Log(text);
			File.WriteAllText(filePath, text);
		}

		static string ParseRegisterFlags(RegisterFlags registerFlags, bool preventInjections)
		{
			registerFlags &= ~RegisterFlags.Parameter;

			if (preventInjections)
			{
				registerFlags |= RegisterFlags.PreventInjections;
			}

			if (registerFlags > RegisterFlags.None)
			{
				registerFlags &= ~RegisterFlags.None;
			}

			var flags = Regex.Split(registerFlags.ToString(), ", ");
			var names = flags.Select(f => "RegisterFlags." + f).ToArray();

			return string.Join(" | ", names);
		}

		static void AppendLine(List<string> target, List<string> source)
		{
			if (source.Count > 0)
			{
				var sourceString = string.Join(Environment.NewLine, source);
				AppendLine(target, sourceString, -INDENT);
			}
		}

		static void AppendLine(List<string> target, string text = "", int offset = 0)
		{
			var indent = Indent(offset);

			if (text == string.Empty)
			{
				indent = text;
			}

			target.Add(indent + text);
		}

		static string Indent(int count = 0)
		{
			count += INDENT;
			const string TAB = "\t";
			var indent = string.Empty;

			for (var i = 0; i < count; i++)
			{
				indent += TAB;
			}

			return indent;
		}
	}
}