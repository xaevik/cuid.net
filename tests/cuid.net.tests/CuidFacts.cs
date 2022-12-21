﻿namespace Xaevik.Cuid.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

[ExcludeFromCodeCoverage]
public class CuidFacts
{
	//   _t       _c   _f   _r
	// c lbqylg5v 0001 08mn 7kmn0t1e
	private const string CuidString = "clbqylg5v000108mn7kmn0t1e";

	private const string InvalidCuidString = "xSQcDXq7N6YTJZ7i1zNXCA==";

	[Fact]
	public void Cuid_ConstructFromString()
	{
		var cuid = new Cuid(CuidString);

		Assert.Equal(CuidString, cuid.ToString());
	}

	[Fact]
	public void Cuid_ConstructFromString_ThrowsFormatException()
	{
		Assert.Throws<FormatException>(() => new Cuid(InvalidCuidString));
	}

	[Fact]
	public void Cuid_Equality()
	{
		var c1 = new Cuid(CuidString);
		var c2 = new Cuid(CuidString);

		Assert.True(c1 == c2);
	}

	[Fact]
	public void Cuid_Equals()
	{
		var c1 = new Cuid(CuidString);
		var c2 = new Cuid(CuidString);

		Assert.True(c1.Equals(c2));
		Assert.True(c1.Equals((object) c2));

		Assert.True(c1.CompareTo(null) > 0);
		Assert.True(c1.CompareTo(c2) == 0);
		Assert.True(c1.CompareTo((object) c2) == 0);

		Assert.True(c1 >= c2);
		Assert.True(c1 <= c2);

		Assert.True(c1.GetHashCode() == c2.GetHashCode());
	}

	[Fact]
	public void Cuid_GreaterThan()
	{
		var c1 = new Cuid("clbqylg5v000108mn7kmn0t1e");
		var c2 = new Cuid("clbqylg5v000208mn7kmn0t1e");

		Assert.True(c2 > c1);
	}

	[Fact]
	public void Cuid_Inequality()
	{
		var c1 = new Cuid();
		var c2 = new Cuid(CuidString);

		Assert.False(c1 == c2);
	}

	[Fact]
	public void Cuid_Json_Deserialize()
	{
		var result = JsonSerializer.Deserialize<Cuid>($"\"{CuidString}\"");

		Assert.Equal(CuidString, result.ToString());
	}

	[Fact]
	public void Cuid_Json_Serialize()
	{
		var cuid = new Cuid(CuidString);

		var result = JsonSerializer.Serialize(cuid);

		Assert.Equal($"\"{CuidString}\"", result);
	}

	[Fact]
	public void Cuid_LessThan()
	{
		var c1 = new Cuid("clbqylg5v000108mn7kmn0t1e");
		var c2 = new Cuid("clbqylg5v000208mn7kmn0t1e");

		Assert.True(c1 < c2);
	}

	[Fact]
	public void Cuid_NewCuid()
	{
		var cuid = Cuid.NewCuid();

		var cuidString = cuid.ToString();

		var result = cuidString.Length == 25
		             && cuidString.All(char.IsLetterOrDigit)
		             && cuid != Cuid.Empty;

		Assert.True(result);
	}

	[Fact]
	public void Cuid_NotEquals()
	{
		var c1 = new Cuid();
		var c2 = new Cuid(CuidString);

		Assert.False(c1.Equals(c2));
		Assert.False(c1.Equals((object) c2));

		Assert.False(c1.CompareTo(c2) == 0);
		Assert.False(c1.CompareTo((object) c2) == 0);

		Assert.False(c1.GetHashCode() == c2.GetHashCode());
	}

	[Fact]
	public void Cuid_Parse()
	{
		var cuid = Cuid.Parse(CuidString);

		Assert.Equal(CuidString, cuid.ToString());
	}

	[Fact]
	public void Cuid_Parse_ThrowsFormatException()
	{
		Assert.Throws<FormatException>(() => Cuid.Parse(InvalidCuidString));
	}

	[Fact]
	public void Cuid_TryParse_Null_ReturnsFalse()
	{
		var result = Cuid.TryParse(null, out var cuid);

		Assert.False(result);
	}

	[Fact]
	public void Cuid_TryParse_ReturnsFalse()
	{
		var result = Cuid.TryParse(InvalidCuidString, out var cuid);

		Assert.False(result);
		Assert.Equal(cuid, Cuid.Empty);
	}

	[Fact]
	public void Cuid_TryParse_ReturnsTrue()
	{
		var result = Cuid.TryParse(CuidString, out var cuid);

		Assert.True(result);
		Assert.Equal(CuidString, cuid.ToString());
	}

	[Fact]
	public void Cuid_Xml_Deserialize()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><cuid>clbqylg5v000108mn7kmn0t1e</cuid>";

		var expected = new Cuid(CuidString);

		var serializer = new XmlSerializer(typeof(Cuid));

		using var stringReader = new StringReader(xml);
		using var xmlReader = XmlReader.Create(stringReader);

		var cuid = serializer.Deserialize(xmlReader);

		Assert.Equal(expected, cuid);
	}

	[Fact]
	public void Cuid_Xml_Serialize()
	{
		const string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?><cuid>clbqylg5v000108mn7kmn0t1e</cuid>";
		var cuid = new Cuid(CuidString);

		var serializer = new XmlSerializer(typeof(Cuid));
		var settings = new XmlWriterSettings { Indent = false, Encoding = new UnicodeEncoding(false, false) };

		using var stringWriter = new StringWriter();
		using var xmlWriter = XmlWriter.Create(stringWriter, settings);

		serializer.Serialize(xmlWriter, cuid);

		Assert.Equal(expected, stringWriter.ToString());
	}
}