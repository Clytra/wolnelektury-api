using WolneLektury.Infrastructure.Integrations.WolneLektury;

namespace WolneLektury.Api.Tests.Unit;

public class UpstreamMappingTests
{
    [Fact]
    public void ToAuthorDto_MapsSlugAndName()
    {
        var upstream = new UpstreamAuthorDto("adam-mickiewicz", "Adam Mickiewicz");
        var dto = UpstreamMapping.ToAuthorDto(upstream);
        Assert.Equal("adam-mickiewicz", dto.Slug);
        Assert.Equal("Adam Mickiewicz", dto.Name);
    }

    [Fact]
    public void ToBookDto_MapsAllFields_AndDerivesSlugFromHrefWhenSlugNull()
    {
        var upstream = new UpstreamBookDto(
            Title: "Pan Tadeusz",
            Slug: null,
            Href: "https://wolnelektury.pl/api/books/pan-tadeusz/",
            Description: "Opis",
            Authors: [new UpstreamAuthorRef("adam-mickiewicz", "Adam Mickiewicz", null)]);
        var dto = UpstreamMapping.ToBookDto(upstream);
        Assert.Equal("pan-tadeusz", dto.Slug);
        Assert.Equal("Pan Tadeusz", dto.Title);
        Assert.Equal("Opis", dto.Description);
        Assert.Single(dto.Authors);
        Assert.Equal("adam-mickiewicz", dto.Authors[0].Slug);
        Assert.Equal("Adam Mickiewicz", dto.Authors[0].Name);
    }

    [Fact]
    public void ToBookDto_UsesSlugWhenPresent()
    {
        var upstream = new UpstreamBookDto(Title: "Dziady", Slug: "dziady");
        var dto = UpstreamMapping.ToBookDto(upstream);
        Assert.Equal("dziady", dto.Slug);
    }
}
