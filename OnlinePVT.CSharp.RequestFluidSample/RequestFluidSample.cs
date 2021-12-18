using OnlinePvt.Gateway.Api.Shared.Client;
using OnlinePvt.Gateway.Api.Shared.Enumeration;
using OnlinePvt.Gateway.Api.Shared.Fluids.FluidParts;
using OnlinePvt.Gateway.Api.Shared.Utility;

namespace OnlinePVT.CSharp.FlashSample
{
    internal static class RequestFluidSample
    {
        public static async Task RunRequestFluidSampleAsync(string? userId, string? accessSecret)
        {
            try
            {
                var client = CreateClient(userId, accessSecret);
                var input = client.GetRequestFluid();
                input.FluidId = new Guid("9E9ABAD5-C6CA-427F-B5E7-15AB3F7CF076");

                var result = await client.RequestFluidAsync(input);

                if (result.ApiStatus == ApiCallResult.Success && result.Fluid is not null)
                {
                    PrintFluidInfo(result.Fluid);
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

        private static OnlinePvtClient CreateClient(string? userId, string? accessSecret)
        {
            if (userId is null)
                throw new ArgumentNullException("ONLINEPVT_USER_ID");
            if (accessSecret is null)
                throw new ArgumentNullException("ONLINEPVT_ACCESS_SECRET");

            return new OnlinePvtClient(new HttpClient(), "https://api.onlinepvt.com", userId, accessSecret);
        }

        private static void PrintExceptionInfo(ExceptionInfo exceptionInfo)
        {
            PrintLine($"Date: {exceptionInfo.Date}");
            PrintLine($"Message Type: {exceptionInfo.MessageType}");
            PrintLine($"Message: {exceptionInfo.Message}");
            PrintLine("");
            PrintLine($"Stack Trace: {exceptionInfo.StackTrace}");
        }

        private static void PrintLine(string input) => Console.WriteLine(input);

        private static void PrintFluidInfo(ApiFluid fluid)
        {
            PrintLine($"Fluid: {fluid.Name}");
            PrintLine($"Comment: {fluid.Comment}");
            var eos = fluid.Eos == EosModel.PCSAFT ? "PC-SAFT" : "coPC-SAFT";
            PrintLine($"EoS: {eos}");
            var model = fluid.SolventCp == CpModel.Polynomial ? "Polynomial" : "DIPPR";
            PrintLine($"Solvent Cp: {model}");
            model = fluid.PolymerCp == CpModel.Polynomial ? "Polynomial" : "DIPPR";
            PrintLine($"Polymer Cp: {model}");

            var ref_point =
                fluid.PropertyReferencePoint == PropertyReferencePoint.Original ? "Original" :
                fluid.PropertyReferencePoint == PropertyReferencePoint.IdealGas ? "Ideal Gas" : "Standard State";
            PrintLine($"Property reference point: {ref_point}");

            PrintLine($"No standard components: {fluid.Standards.Count}");
            PrintLine($"No polymers: {fluid.Polymers.Count}");
        }

    }
}
