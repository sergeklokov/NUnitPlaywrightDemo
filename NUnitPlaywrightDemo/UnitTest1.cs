using Microsoft.Extensions.Configuration;

namespace NUnitPlaywrightDemo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private TestSettings _settings;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Load configuration
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _settings = config.GetSection("TestSettings").Get<TestSettings>();

            // Initialize Playwright
            //var playwright = await Playwright.CreateAsync();
            //_apiContext = await playwright.APIRequest.NewContextAsync(new()
            //{
            //    IgnoreHTTPSErrors = true // For self-signed certificate
            //});
        }

        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            //await Page.GotoAsync("https://playwright.dev");
            await Page.GotoAsync(_settings.BaseUrl);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

            // create a locator
            var getStarted = Page.Locator("text=Get Started");

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        }
    }
}
