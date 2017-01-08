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

using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using ExtendedXmlSerialization.Core;

namespace ExtendedXmlSerialization.Converters.Legacy
{
    class LegacyRootConverter : DecoratedConverter
    {
        private readonly ISerializationToolsFactory _factory;

        public LegacyRootConverter(ISerializationToolsFactory factory, IConverter converter) : base(converter)
        {
            _factory = factory;
        }

        public override void Write(XmlWriter writer, object instance)
        {
            var configuration = _factory.GetConfiguration(instance.GetType());
            if (configuration != null)
            {
                if (configuration.Version > 0)
                {
                    writer.WriteAttributeString(ExtendedXmlSerializer.Version,
                                                configuration.Version.ToString(CultureInfo.InvariantCulture));
                }

                if (configuration.IsCustomSerializer)
                {
                    new CustomElementWriter(configuration).Write(writer, instance);
                    return;
                }
            }

            base.Write(writer, instance);
        }

        public override object Read(XElement element, Typed? hint = null)
        {
            var configuration = _factory.GetConfiguration(hint);
            if (configuration != null)
            {
                // Run migrator if exists
                if (configuration.Version > 0)
                {
                    configuration.Map(hint, element);
                }

                if (configuration.IsCustomSerializer)
                {
                    return configuration.ReadObject(element);
                }
            }

            return base.Read(element, hint);
        }
    }
}