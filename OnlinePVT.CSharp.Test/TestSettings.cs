using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace OnlinePVT.Test
{
  [TestFixture]
  public class TestSettings
  {

    /// <summary>
    /// Ensures appsettings does not get comitted
    /// </summary>
    [Test]  
    public void TestSettingsEmpty()
    {
      var basePath = Path.GetFullPath(Path.Combine(System.IO.Directory.GetCurrent‌​Directory(), "..//..//..//..//"));
      var settingFiles = new List<string>
      {
        Path.Combine(basePath, "OnlinePVT.CSharp.CloudPointSample", "appsettings.json"),
        Path.Combine(basePath, "OnlinePVT.CSharp.FlashSample", "appsettings.json"),
        Path.Combine(basePath, "OnlinePVT.CSharp.PhaseDiagramSample", "appsettings.json"),
        Path.Combine(basePath, "OnlinePVT.CSharp.RequestFluidSample", "appsettings.json"),
      };

      foreach (var item in settingFiles)
      {
        var configuration = InitConfiguration(item);

        var config = configuration.GetSection("AppConfig");
        var userId = config.GetSection("ONLINEPVT_USER_ID").Value;
        var accessSecret = config.GetSection("ONLINEPVT_ACCESS_SECRET").Value;

        Assert.IsTrue(string.IsNullOrEmpty(userId));
        Assert.IsTrue(string.IsNullOrEmpty(accessSecret));
      }

    }

    IConfiguration InitConfiguration(string file)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile(file)
          .Build();
      return config;
    }
  }
}