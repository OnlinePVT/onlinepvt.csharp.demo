using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
var configurationRoot = builder.Build();
var config = configurationRoot.GetSection("AppConfig");
var userId = config.GetSection("ONLINEPVT_USER_ID").Value;
var accessSecret = config.GetSection("ONLINEPVT_ACCESS_SECRET").Value;

// we intentionally block the thread until the calculation is complete for the sake of the sample
OnlinePVT.CSharp.FlashSample.RequestFluidSample.RunRequestFluidSampleAsync(userId, accessSecret).Wait();
