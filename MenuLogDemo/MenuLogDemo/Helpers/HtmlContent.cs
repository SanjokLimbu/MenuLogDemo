using System;
using System.Collections.Generic;
using System.Text;

namespace MenuLogDemo.Helpers
{
    public class HtmlContent
    {
        public class Table : IDisposable
        {
            private readonly StringBuilder _stringBuilder;

            public Table(StringBuilder stringBuilder, string id = "default", string classValue = "")
            {
                _stringBuilder = stringBuilder;
                _stringBuilder.Append($"<table id=\"{id}\" class=\"{classValue}\">\n");
            }
            public void Dispose()
            {
                _stringBuilder.Append("</table>");
            }
            public Row AddRow()
            {
                return new Row(_stringBuilder);
            }
            public Row AddHeaderRow()
            {
                return new Row(_stringBuilder, true);
            }
            public void StartTableBody()
            {
                _stringBuilder.Append("</tbody>");
            }
            public void EndTableBody()
            {
                _stringBuilder.Append("</tbody");
            }
        }
        public class Row : IDisposable
        {
            private readonly StringBuilder _stringBuilder;
            public bool _isHeader;
            public Row(StringBuilder stringBuilder, bool isHeader = false)
            {
                _stringBuilder = stringBuilder;
                _isHeader = isHeader;
                if(_isHeader = isHeader)
                {
                    _stringBuilder.Append("<thead>\n");
                }
                _stringBuilder.Append("\t<tr>\n");
            }
            public void Dispose()
            {
                _stringBuilder.Append("\t</tr>\n");
                if (_isHeader)
                {
                    _stringBuilder.Append("</thead>\n");
                }
            }
            public void AddCell(string innerText)
            {
                _stringBuilder.Append("\t\t<td>\n");
                _stringBuilder.Append("\t\t\t" + innerText);
                _stringBuilder.Append("\t\t</td>\n");
            }
        }
    }
}
