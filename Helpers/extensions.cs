using Microsoft.AspNetCore.Http;

namespace LocalCellars.API.Helpers
{
    public static class extensions
    {
        //Extension method to add additional headers to our current http response obj
        public static void AddApplicationError(this HttpResponse response, string message){
            //In the event of an exception, send back this new header called Application Error
            //With the message as its value
            //Then other two headers are simply to allow this new header to be displayed
            response.Headers.Add("Application-Error", message);
            //And allow origin from any endpoint
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-ControlAllow-Origin", "*");
        }
    }
}