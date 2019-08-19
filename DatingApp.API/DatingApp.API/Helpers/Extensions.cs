using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        public static void AddPagination(this HttpResponse response,
             int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
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
