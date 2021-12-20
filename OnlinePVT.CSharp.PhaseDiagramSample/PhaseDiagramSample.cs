using OnlinePvt.Gateway.Api.Shared.ApiOutput.Phasediagram;
using OnlinePvt.Gateway.Api.Shared.Calculations.PhaseDiagram;
using OnlinePvt.Gateway.Api.Shared.Client;
using OnlinePvt.Gateway.Api.Shared.Enumeration;
using OnlinePvt.Gateway.Api.Shared.Input;
using OnlinePvt.Gateway.Api.Shared.Utility;

namespace OnlinePVT.CSharp.FlashSample
{
  internal static class PhaseDiagramSample
  {
    public static async Task RunPhaseDiagramSampleAsync(string userId, string accessSecret)
    {
      try
      {
        var client = CreateClient(userId, accessSecret);
        var input = CreateInput(client);

        var result = await client.CallCalculationPhasediagramStandardAsync(input);

        if (result.ApiStatus == ApiCallResult.Success && result.Curve is not null)
        {
          PrintPhaseDiagramResult(result.Curve);
        }
        else if (result.ExceptionInfo is not null)
          PrintExceptionInfo(result.ExceptionInfo);
      }
      catch (Exception ex)
      {
        PrintLine($"Message: {ex.Message}");
        PrintLine($"Stack Trace: {ex.StackTrace}");
      }
    }

    static OnlinePvtClient CreateClient(string userId, string accessSecret)
    {
      return new OnlinePvtClient(new HttpClient(), "https://api.onlinepvt.com", userId, accessSecret);
    }

    static PhasediagramFixedTemperaturePressureCalculationInput CreateInput(OnlinePvtClient client)
    {
      var input = client.GetPhasediagamStandardInput();
      input.FluidId = new Guid("9E9ABAD5-C6CA-427F-B5E7-15AB3F7CF076");
      input.VLLE = true;
      input.Components = new List<CalculationComposition> {
                new CalculationComposition { Mass = 0.78 },
                new CalculationComposition { Mass = 0.02 },
                new CalculationComposition { Mass = 0.2 } };
      return input;
    }

    static void PrintExceptionInfo(ExceptionInfo exceptionInfo)
    {
      PrintLine($"Date: {exceptionInfo.Date}");
      PrintLine($"Message Type: {exceptionInfo.MessageType}");
      PrintLine($"Message: {exceptionInfo.Message}");
      PrintLine();
      PrintLine($"Stack Trace: {exceptionInfo.StackTrace}");
    }

    static void PrintValue(double input) => PrintValue(input.ToString());
    static void PrintValue(string input) => Console.Write(input.PadRight(25));
    static void PrintLine(string input = "") => Console.WriteLine(input);

    static void PrintPhaseDiagramResult(ApiOutputPhasediagram result)
    {
      PrintLinePoints("Phase Envelope", result.TemperatureUnits, result.PressureUnits, result.Phaseenvelope);
      PrintLinePoints("VLLE", result.TemperatureUnits, result.PressureUnits, result.Vlle);
      PrintLinePoints("SLE", result.TemperatureUnits, result.PressureUnits, result.SLE);
      PrintLinePoints("SLVE", result.TemperatureUnits, result.PressureUnits, result.Slve);
    }

    static void PrintLinePoints(string title, string temperatureUnit, string pressureUnit, IEnumerable<ApiOutputPhasediagramPoint> points)
    {
      if (!points.Any())
        return;

      PrintLine();
      PrintLine(title);
      PrintValue("Label");
      PrintValue($"Temperature [{temperatureUnit}]");
      PrintValue($"Pressure [{pressureUnit}]");
      PrintLine();
      foreach (var point in points)
      {
        if (point.Label != null)
          PrintValue(point.Label);
        else
          PrintValue(string.Empty);
        PrintValue(point.Temperature);
        PrintValue(point.Pressure);
        PrintLine();
      }
      PrintLine();
    }
  }
}
