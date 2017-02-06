// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ExtendedXmlSerialization.Core;

namespace ExtendedXmlSerialization.ConverterModel.Xml
{
	class Prefixes : IPrefixes
	{
		public static Prefixes Default { get; } = new Prefixes();

		Prefixes() : this(WellKnownNamespaces.Default,
		                  WellKnownNamespaces.Default.Values.ToDictionary(x => XNamespace.Get(x.Identifier), x => x.Prefix),
		                  WellKnownNamespaces.Default.Values.ToDictionary(x => x.Prefix, x => XNamespace.Get(x.Identifier)),
		                  PrefixProvider.Default) {}

		readonly IDictionary<Assembly, Namespace> _known;
		readonly IDictionary<XNamespace, string> _names;
		readonly IDictionary<string, XNamespace> _namespaces;
		readonly IPrefixProvider _provider;

		public Prefixes(IDictionary<Assembly, Namespace> known, IDictionary<XNamespace, string> names,
		                IDictionary<string, XNamespace> namespaces,
		                IPrefixProvider provider)
		{
			_known = known;
			_names = names;
			_namespaces = namespaces;
			_provider = provider;
		}

		public string Get(TypeInfo parameter)
			=> _known.GetStructure(parameter.Assembly)?.Prefix ?? _provider.Get(parameter.AssemblyQualifiedName);

		public string Get(XName parameter) => _names.Get(parameter.NamespaceName) ?? _provider.Get(parameter.NamespaceName);
		public XNamespace Get(string parameter) => _namespaces.Get(parameter);
	}
}