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

using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.ContentModel.Parsing;
using ExtendedXmlSerializer.ContentModel.Xml;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ExtensionModel.Expressions;
using ExtendedXmlSerializer.TypeModel;

namespace ExtendedXmlSerializer.ExtensionModel.Markup
{
	sealed class MarkupExtensionContainer : ReferenceCacheBase<IXmlReader, IMarkupExtensionPartsEvaluator>, IMarkupExtensionContainer
	{
		readonly IEvaluator _evaluator;
		readonly IReflectionParsers _parsers;
		readonly ITypeMembers _members;
		readonly IMemberAccessors _accessors;
		readonly IConstructors _constructors;
		readonly System.IServiceProvider _provider;

		public MarkupExtensionContainer(IEvaluator evaluator, IReflectionParsers parsers, ITypeMembers members,
		                                IMemberAccessors accessors, IConstructors constructors, System.IServiceProvider provider)
		{
			_evaluator = evaluator;
			_parsers = parsers;
			_members = members;
			_accessors = accessors;
			_constructors = constructors;
			_provider = provider;
		}

		protected override IMarkupExtensionPartsEvaluator Create(IXmlReader parameter)
			=> new MarkupExtensionPartsEvaluator(parameter, _provider, _evaluator, _parsers.Get(parameter), _members, _accessors, _constructors);
	}
}