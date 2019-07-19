using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API
{
    public static class Extensions
    {
        public static void AddAplicactionError(this HttpResponse response, string messange)
        {
            response.Headers.Add("Applicaction-Error", messange);
            response.Headers.Add("Access-Control-Expose-Heardes", "Application-Error");
            response.Headers.Add("Access-Control-Allows-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDataTime)
        {
            var age = DateTime.Today.Year - theDataTime.Year;
            if (theDataTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
    }
}
